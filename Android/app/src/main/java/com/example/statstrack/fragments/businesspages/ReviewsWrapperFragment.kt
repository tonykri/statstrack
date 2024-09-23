package com.example.statstrack.fragments.businesspages

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.reviewspage.AddReviewFragment
import com.example.statstrack.fragments.businesspages.reviewspage.ReviewFragment
import com.example.statstrack.fragments.common.CouponFragment
import com.example.statstrack.helper.apiCalls.BusinessService
import com.example.statstrack.helper.apiCalls.ReviewsService
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.example.statstrack.helper.apiCalls.dto.response.ReviewResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class ReviewsWrapperFragment(private val businessId: UUID, private val belongsToUser: Boolean) : Fragment() {

    private lateinit var layout: LinearLayout
    private var reviews: List<ReviewResponse> = listOf()

    private val reviewsService: ReviewsService by lazy {
        ReviewsService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_reviews_wrapper, container, false)

        layout = view.findViewById(R.id.reviewsWrapperLayout)

        addReviews()

        return view
    }

    fun addReviews() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        lifecycleScope.launch(Dispatchers.IO) {
            reviewsService.getReviews(businessId) { data ->
                Log.d("Reviews", data.toString())
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        reviews = data
                        Log.d("Reviews", reviews.toString())

                        layout.removeAllViews()

                        val fragment = AddReviewFragment(businessId)
                        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment)

                        Log.d("Fragments", reviews.size.toString())
                        for (review in reviews) {
                            val fragment = ReviewFragment(review, businessId, belongsToUser)
                            fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment)
                        }

                        fragmentTransaction.commit()
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

}