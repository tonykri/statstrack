package com.example.statstrack.fragments.common

import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import androidx.viewpager2.widget.ViewPager2
import com.bumptech.glide.Glide
import com.bumptech.glide.load.model.GlideUrl
import com.bumptech.glide.load.model.LazyHeaders
import com.example.statstrack.R
import com.example.statstrack.activities.BusinessActivity
import com.example.statstrack.activities.HomeActivity
import com.example.statstrack.helper.SliderPagerAdapter
import com.example.statstrack.helper.apiCalls.HomePageService
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class BusinessFragment(private val business: BusinessResponse) : Fragment() {

    private lateinit var viewPager: ViewPager2
    private lateinit var title: TextView
    private lateinit var description: TextView
    private lateinit var category: TextView
    private lateinit var address: TextView
    private lateinit var reviews: TextView
    private lateinit var stars: TextView

    private val homePageService: HomePageService by lazy {
        HomePageService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_business, container, false)

        viewPager = view.findViewById(R.id.businessFragmentViewPager)
        title = view.findViewById(R.id.businessFragmentTitle)
        description = view.findViewById(R.id.businessFragmentDescription)
        category = view.findViewById(R.id.businessFragmentCategory)
        address = view.findViewById(R.id.businessFragmentAddress)
        reviews = view.findViewById(R.id.businessFragmentReviews)
        stars = view.findViewById(R.id.businessFragmentStars)

        initData()

        address.setOnClickListener{
            val address = address.text.toString()
            openGoogleMaps(address)
        }

        view.setOnClickListener{
            val intent = Intent(requireContext(), BusinessActivity::class.java)
            intent.putExtra("businessId", business.id.toString())
            intent.putExtra("userId", business.userId.toString())
            Log.d("USERIDDD", business.userId.toString())
            startActivity(intent)
        }

        return view
    }

    private fun openGoogleMaps(address: String) {
        val mapIntent = Intent(Intent.ACTION_VIEW, Uri.parse("geo:0,0?q=$address"))
        mapIntent.setPackage("com.google.android.apps.maps")
        if (mapIntent.resolveActivity(requireContext().packageManager) != null) {
            startActivity(mapIntent)
        }
    }


    private fun initData() {
        title.text = business.brand
        description.text = business.description
        category.text = business.category.toString().replace("_", " ")
        address.text = business.address
        reviews.text = business.reviews.toString()
        stars.text = business.stars.toString()

        val images: MutableList<Bitmap> = mutableListOf()
        if (business.photos.isEmpty()) {
            val drawableId = R.drawable.no_image
            val bitmap = BitmapFactory.decodeResource(resources, drawableId)
            images.add(bitmap)
        }
        for (photo in business.photos) {
            lifecycleScope.launch(Dispatchers.IO) {
                homePageService.getBusinessPhoto(business.id, photo.photoId) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data != null) {
                            images.add(data)
                        } else {
                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }
        val adapter = SliderPagerAdapter(requireContext(), images)
        viewPager.adapter = adapter
    }

}