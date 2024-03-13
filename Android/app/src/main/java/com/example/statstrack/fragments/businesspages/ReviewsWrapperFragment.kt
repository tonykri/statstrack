package com.example.statstrack.fragments.businesspages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.reviewspage.AddReviewFragment
import com.example.statstrack.fragments.businesspages.reviewspage.ReviewFragment
import com.example.statstrack.fragments.common.CouponFragment

class ReviewsWrapperFragment : Fragment() {

    private lateinit var layout: LinearLayout

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

        view.postDelayed({
            addReviews()
        }, 2000)

        return view
    }

    private fun addReviews() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        layout.removeAllViews()

        val fragment = AddReviewFragment()
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment)

        val fragment1 = ReviewFragment()
        val fragment2 = ReviewFragment()
        val fragment3 = ReviewFragment()
        val fragment4 = ReviewFragment()
        val fragment5 = ReviewFragment()
        val fragment6 = ReviewFragment()
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment1)
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment2)
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment3)
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment4)
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment5)
        fragmentTransaction.add(R.id.reviewsWrapperLayout, fragment6)

        fragmentTransaction.commit()
    }

}