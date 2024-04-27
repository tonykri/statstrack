package com.example.statstrack.fragments.common

import android.content.Intent
import android.graphics.Bitmap
import android.os.Bundle
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
        address = view.findViewById(R.id.businessFragmentAddress)
        reviews = view.findViewById(R.id.businessFragmentReviews)
        stars = view.findViewById(R.id.businessFragmentStars)

        initData()

        view.setOnClickListener{
            val intent = Intent(requireContext(), BusinessActivity::class.java)
            startActivity(intent)
        }

        return view
    }

    private fun initData() {
        title.text = business.brand
        description.text = business.description
        address.text = business.address
        reviews.text = business.reviews.toString()
        stars.text = business.stars.toString()

        if(business.photos.isEmpty())
            return

        val images: MutableList<Bitmap> = mutableListOf()
        for (photo in business.photos) {
            lifecycleScope.launch(Dispatchers.IO) {
                homePageService.getBusinessPhoto(business.id, photo.photoId) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data != null) {
                            images.add(data)
                        } else {
//                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }
        val adapter = SliderPagerAdapter(requireContext(), images)
        viewPager.adapter = adapter
    }

}