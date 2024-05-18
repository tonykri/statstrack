package com.example.statstrack.fragments.businesspages.reviewspage

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.RatingBar
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.ReviewsWrapperFragment
import com.example.statstrack.helper.apiCalls.ReviewsService
import com.example.statstrack.helper.apiCalls.dto.request.ReviewCreateRequest
import com.example.statstrack.helper.apiCalls.dto.request.ReviewUpdateRequest
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID
import kotlin.math.roundToInt

class AddReviewFragment(private val businessId: UUID) : Fragment() {

    private val reviewsService: ReviewsService by lazy {
        ReviewsService(requireContext())
    }

    private lateinit var content: EditText
    private lateinit var stars: RatingBar

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_add_review, container, false)

        content = view.findViewById(R.id.addReviewFragmentContent)
        stars = view.findViewById(R.id.addReviewFragmentStars)

        view.findViewById<Button>(R.id.addReviewFragmentSubmit).setOnClickListener{
            val req = ReviewCreateRequest(businessId, stars.rating.roundToInt(), content.text.toString())
            lifecycleScope.launch(Dispatchers.IO) {
                reviewsService.postReview(req) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            Toast.makeText(requireContext(), "Review Posted", Toast.LENGTH_LONG).show()
                            (parentFragment as? ReviewsWrapperFragment)?.addReviews()
                        } else {
                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }

        return view
    }

}