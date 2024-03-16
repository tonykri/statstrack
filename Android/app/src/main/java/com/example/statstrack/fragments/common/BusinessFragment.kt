package com.example.statstrack.fragments.common

import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.viewpager2.widget.ViewPager2
import com.example.statstrack.R
import com.example.statstrack.activities.BusinessActivity
import com.example.statstrack.activities.HomeActivity
import com.example.statstrack.helper.SliderPagerAdapter

class BusinessFragment : Fragment() {


    private val images = listOf(
        R.drawable.ic_business,
        R.drawable.ic_coupon,
        R.drawable.ic_google
    )

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_business, container, false)

        val viewPager: ViewPager2 = view.findViewById(R.id.businessFragmentViewPager)
        val adapter = SliderPagerAdapter(requireContext(), images)
        viewPager.adapter = adapter

        view.setOnClickListener{
            val intent = Intent(requireContext(), BusinessActivity::class.java)
            startActivity(intent)
        }

        return view
    }

}