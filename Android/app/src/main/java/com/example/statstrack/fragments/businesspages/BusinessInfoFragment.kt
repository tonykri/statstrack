package com.example.statstrack.fragments.businesspages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.BusinessService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class BusinessInfoFragment(private val businessId: UUID) : Fragment() {
    private lateinit var title: TextView
    private lateinit var description: TextView
    private lateinit var category: TextView
    private lateinit var address: TextView
    private lateinit var reviews: TextView
    private lateinit var stars: TextView

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
        val view = inflater.inflate(R.layout.fragment_business_info, container, false)

        title = view.findViewById(R.id.businessInfoFragmentTitle)
        description = view.findViewById(R.id.businessInfoFragmentDescription)
        category = view.findViewById(R.id.businessInfoFragmentCategory)
        address = view.findViewById(R.id.businessInfoFragmentAddress)
        reviews = view.findViewById(R.id.businessInfoFragmentReviews)
        stars = view.findViewById(R.id.businessInfoFragmentStars)

        initData()

        return view
    }

    private fun initData() {
        lifecycleScope.launch(Dispatchers.IO) {
            businessService.getBusiness(businessId) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        title.text = data.brand
                        description.text = data.description
                        category.text = data.category.toString().replace("_", " ")
                        address.text = data.address
                        reviews.text = data.reviews.toString()
                        stars.text = data.stars.toString()
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

}