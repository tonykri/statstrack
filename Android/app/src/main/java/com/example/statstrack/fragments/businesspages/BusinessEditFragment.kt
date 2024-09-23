package com.example.statstrack.fragments.businesspages

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.activity.result.ActivityResultCallback
import androidx.activity.result.PickVisualMediaRequest
import androidx.activity.result.contract.ActivityResultContracts
import androidx.activity.result.contract.ActivityResultContracts.PickVisualMedia.ImageOnly
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.BrowserActivity
import com.example.statstrack.fragments.businesspages.editpage.PhotoEditFragment
import com.example.statstrack.helper.apiCalls.BusinessService
import com.example.statstrack.helper.apiCalls.dto.request.BusinessUpdateRequest
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.Callback
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import java.io.IOException
import java.io.InputStream
import java.net.URLEncoder
import java.util.UUID


class BusinessEditFragment(private val businessId: UUID) : Fragment() {

    companion object {
        private const val REQUEST_IMAGE_PICK = 1
    }

    private val apiKey = "AIzaSyDMI0DbGwiFi5O2i9C9RKLqLIKtoJi9xSE"

    private lateinit var sharedPref: SharedPreferences

    private lateinit var title: EditText
    private lateinit var description: EditText
    private lateinit var address: EditText
    private lateinit var photosLayout: LinearLayout
    private lateinit var categoriesSpinner: Spinner
    private lateinit var saveBtn: Button
    private lateinit var businessEditFragmentLocationBtn: Button
    private lateinit var businessEditFragmentRenew: TextView
    private lateinit var addImageBtn: ImageView
    private var latitude = 0.0
    private var longitude = 0.0

    private val businessService: BusinessService by lazy {
        BusinessService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_business_edit, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        title = view.findViewById(R.id.businessEditFragmentTitle)
        description = view.findViewById(R.id.businessEditFragmentDescription)
        address = view.findViewById(R.id.businessEditFragmentAddress)
        photosLayout = view.findViewById(R.id.businessEditFragmentPhotosLayout)
        categoriesSpinner = view.findViewById(R.id.businessEditFragmentSpinner)
        saveBtn = view.findViewById(R.id.businessEditFragmentSaveBtn)
        businessEditFragmentLocationBtn = view.findViewById(R.id.businessEditFragmentLocationBtn)
        businessEditFragmentRenew = view.findViewById(R.id.businessEditFragmentRenew)
        addImageBtn = view.findViewById(R.id.businessEditFragmentAddPhotoBtn)
        initPhotos()
        initSpinner()

        saveBtn.setOnClickListener{
            if(latitude == 0.0 && longitude == 0.0) {
                Toast.makeText(requireContext(), "Select a location", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }
            saveBtn.isEnabled = false
            lifecycleScope.launch(Dispatchers.IO) {
                val dto = BusinessUpdateRequest(businessId, title.text.toString(), description.text.toString(), categoriesSpinner.selectedItem.toString(),
                    address.text.toString(), latitude, longitude)
                businessService.updateBusiness(dto) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data)
                            Toast.makeText(requireContext(), "Business Updated", Toast.LENGTH_LONG).show()
                        else
                            Toast.makeText(requireContext(), "Error occurred", Toast.LENGTH_LONG).show()
                        saveBtn.isEnabled = true
                    }
                }
            }
        }

        businessEditFragmentLocationBtn.setOnClickListener{
            getCoordinatesFromAddress(address.text.toString()) { coordinates ->
                coordinates?.let {
                    println("Latitude: ${it.lat}, Longitude: ${it.lng}")
                    latitude = it.lat
                    longitude = it.lng
                } ?: run {
                    println("Coordinates not found")
                }
            }
        }

        businessEditFragmentRenew.setOnClickListener{
            val url = "http://10.0.2.2:4005/pay?token=${sharedPref.getString("id", "")}&businessId=${businessId.toString()}"
            val intent = Intent(requireContext(), BrowserActivity::class.java)

            intent.putExtra("url", url)

            startActivity(intent)
        }

        addImageBtn.setOnClickListener {
            pickMedia.launch(
                PickVisualMediaRequest.Builder()
                    .setMediaType(ActivityResultContracts.PickVisualMedia.ImageOnly)
                    .build()
            )
        }


        return view
    }

    private val selectedImageUris = mutableListOf<Uri>()
    private val MAX_PHOTO_SELECTION = 7

    private val pickMedia = registerForActivityResult(ActivityResultContracts.PickMultipleVisualMedia()) { uris ->
        if (uris.isNotEmpty()) {
            if (uris.size > MAX_PHOTO_SELECTION) {
                Log.d("PhotoPicker", "Too many photos selected. Only the first $MAX_PHOTO_SELECTION will be processed.")
                selectedImageUris.clear()
                selectedImageUris.addAll(uris.take(MAX_PHOTO_SELECTION))
            } else {
                selectedImageUris.clear()
                selectedImageUris.addAll(uris)
            }
            Log.d("PhotoPicker", "Selected URIs: $uris")
            for (photo in uris) {
                lifecycleScope.launch(Dispatchers.IO) {
                    val bitmap = uriToBitmap(photo) ?: return@launch

                    businessService.addBusinessPhoto(requireContext(), businessId, bitmap) { success ->

                        }
                    }
                }
        } else {
            Log.d("PhotoPicker", "No media selected")
        }
    }

    private fun uriToBitmap(uri: Uri): Bitmap? {
        return try {
            val inputStream: InputStream? = requireContext().contentResolver.openInputStream(uri)
            BitmapFactory.decodeStream(inputStream)
        } catch (e: Exception) {
            e.printStackTrace()
            null
        }
    }

    private fun getCoordinatesFromAddress(address: String, callback: (Coordinates?) -> Unit) {
        val encodedAddress = URLEncoder.encode(address, "UTF-8")
        val url = "https://maps.googleapis.com/maps/api/geocode/json?address=$encodedAddress&key=$apiKey"

        val request = Request.Builder().url(url).build()
        val client = OkHttpClient()

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: okhttp3.Call, e: IOException) {
                e.printStackTrace()
                callback(null)
            }

            override fun onResponse(call: okhttp3.Call, response: Response) {
                response.body?.let { responseBody ->
                    val json = responseBody.string()
                    Log.d("RES", json)
                    val moshi = Moshi.Builder().add(KotlinJsonAdapterFactory()).build()
                    val adapter = moshi.adapter(GeocodingResponse::class.java)
                    val geocodingResponse = adapter.fromJson(json)

                    val location = geocodingResponse?.results?.firstOrNull()?.geometry?.location
                    callback(location)
                } ?: callback(null)
            }
        })
    }

    data class GeocodingResponse(val results: List<Result>)
    data class Result(val geometry: Geometry)
    data class Geometry(val location: Coordinates)
    data class Coordinates(val lat: Double, val lng: Double)

    private fun initPhotos() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        photosLayout.removeAllViews()

        lifecycleScope.launch(Dispatchers.IO) {
            businessService.getBusinessPhotos(businessId) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (!data.isNullOrEmpty()) {
                        for (photoId in data) {
                            val fragment = PhotoEditFragment(businessId, photoId)
                            fragmentTransaction.add(photosLayout.id, fragment)
                        }
                        fragmentTransaction.commit()
                    }
                }
            }
        }
    }

    private fun initSpinner() {
        val data: Array<String> = resources.getStringArray(R.array.business_category_array)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        categoriesSpinner.adapter = adapter
    }

}