package com.example.statstrack.fragments.businesspages

import android.app.Activity
import android.content.Intent
import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.Spinner
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.editpage.PhotoEditFragment
import com.example.statstrack.fragments.businesspages.reviewspage.AddReviewFragment
import com.example.statstrack.fragments.businesspages.reviewspage.ReviewFragment
import com.example.statstrack.helper.apiCalls.BusinessService
import com.example.statstrack.helper.apiCalls.dto.request.BusinessUpdateRequest
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class BusinessEditFragment(private val businessId: UUID) : Fragment() {

    companion object {
        private const val REQUEST_IMAGE_PICK = 1
    }

    private lateinit var title: EditText
    private lateinit var description: EditText
    private lateinit var address: EditText
    private lateinit var photosLayout: LinearLayout
    private lateinit var categoriesSpinner: Spinner
    private lateinit var saveBtn: Button
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

        title = view.findViewById(R.id.businessEditFragmentTitle)
        description = view.findViewById(R.id.businessEditFragmentDescription)
        address = view.findViewById(R.id.businessEditFragmentAddress)
        photosLayout = view.findViewById(R.id.businessEditFragmentPhotosLayout)
        categoriesSpinner = view.findViewById(R.id.businessEditFragmentSpinner)
        saveBtn = view.findViewById(R.id.businessEditFragmentSaveBtn)
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

        addImageBtn.setOnClickListener {
            val intent = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
            startActivityForResult(intent, REQUEST_IMAGE_PICK)
        }


        return view
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if (requestCode == REQUEST_IMAGE_PICK && resultCode == Activity.RESULT_OK) {
            val imageUri: Uri? = data?.data
            imageUri?.let {
                val bitmap = MediaStore.Images.Media.getBitmap(requireContext().contentResolver, it)
                lifecycleScope.launch(Dispatchers.IO) {
                    businessService.addBusinessPhoto(requireContext(), businessId, bitmap) { data ->
                        lifecycleScope.launch(Dispatchers.Main) {
                            if (data)
                                Toast.makeText(requireContext(), "Photo added", Toast.LENGTH_LONG).show()
                            else
                                Toast.makeText(requireContext(), "An error occurred", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }
    }

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