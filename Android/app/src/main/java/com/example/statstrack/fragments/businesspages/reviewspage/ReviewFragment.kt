package com.example.statstrack.fragments.businesspages.reviewspage

import android.content.Context
import android.content.SharedPreferences
import android.graphics.drawable.BitmapDrawable
import android.graphics.drawable.Drawable
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.FrameLayout
import android.widget.LinearLayout
import android.widget.ProgressBar
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.core.view.isVisible
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.ReviewsWrapperFragment
import com.example.statstrack.helper.apiCalls.ReviewsService
import com.example.statstrack.helper.apiCalls.dto.request.ReviewCreateRequest
import com.example.statstrack.helper.apiCalls.dto.request.ReviewResponseRequest
import com.example.statstrack.helper.apiCalls.dto.request.ReviewUpdateRequest
import com.example.statstrack.helper.apiCalls.dto.response.ReviewResponse
import de.hdodenhof.circleimageview.CircleImageView
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID
import kotlin.math.roundToInt

class ReviewFragment(private val review: ReviewResponse, private val businessId: UUID, private val businessBelongsToUser: Boolean) : Fragment() {

    private lateinit var sharedPref: SharedPreferences
    private val reviewsService: ReviewsService by lazy {
        ReviewsService(requireContext())
    }

    private lateinit var reviewLayout: LinearLayout
    private lateinit var reviewerName: TextView
    private lateinit var reviewContent: TextView
    private lateinit var reviewDate: TextView
    private lateinit var reviewStars: TextView

    private lateinit var reviewEditLayout: LinearLayout
    private lateinit var reviewEditText: EditText
    private lateinit var reviewEditStars: RatingBar
    private lateinit var reviewDeleteBtn: Button
    private lateinit var reviewSaveBtn: Button

    private lateinit var replyEdit: CircleImageView

    private lateinit var responseLayout: LinearLayout
    private lateinit var responseContent: TextView
    private lateinit var responseDate: TextView

    private lateinit var responseEditLayout: LinearLayout
    private lateinit var responseEditText: EditText
    private lateinit var responseDeleteBtn: Button
    private lateinit var responseSaveBtn: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_review, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        reviewLayout = view.findViewById(R.id.reviewFragmentShowReviewLayout)
        reviewerName = view.findViewById(R.id.reviewFragmentName)
        reviewContent = view.findViewById(R.id.reviewFragmentReviewContent)
        reviewDate = view.findViewById(R.id.reviewFragmentReviewDateModified)
        reviewStars = view.findViewById(R.id.reviewFragmentReviewStars)

        reviewEditLayout = view.findViewById(R.id.reviewFragmentEditReviewLayout)
        reviewEditText = view.findViewById(R.id.reviewFragmentReviewEdit)
        reviewEditStars = view.findViewById(R.id.reviewFragmentReviewEditStars)
        reviewSaveBtn = view.findViewById(R.id.reviewFragmentReviewEditBtn)
        reviewDeleteBtn = view.findViewById(R.id.reviewFragmentReviewDeleteBtn)

        replyEdit = view.findViewById(R.id.reviewFragmentReplyEdit)

        responseLayout = view.findViewById(R.id.reviewFragmentShowResponseLayout)
        responseContent = view.findViewById(R.id.reviewFragmentResponseContent)
        responseDate = view.findViewById(R.id.reviewFragmentResponseDateModified)

        responseEditLayout = view.findViewById(R.id.reviewFragmentEditResponseLayout)
        responseEditText = view.findViewById(R.id.reviewFragmentResponseEdit)
        responseDeleteBtn = view.findViewById(R.id.reviewFragmentResponseDeleteBtn)
        responseSaveBtn = view.findViewById(R.id.reviewFragmentResponseEditBtn)

        initView()
        replyEdit.setOnClickListener{
            if (businessBelongsToUser) {
                if (editIsOpen(replyEdit.drawable)) {
                    responseLayout.isVisible = review.response != null
                    responseEditLayout.isVisible = false
                    replyEdit.setImageResource(R.drawable.ic_reply)
                } else {
                    responseLayout.isVisible = false
                    responseEditLayout.isVisible = true
                    replyEdit.setImageResource(R.drawable.ic_close)
                }
            }else {
                if (editIsOpen(replyEdit.drawable)) {
                    reviewLayout.isVisible = true
                    reviewEditLayout.isVisible = false
                    replyEdit.setImageResource(R.drawable.ic_pencil)
                } else {
                    reviewLayout.isVisible = false
                    reviewEditLayout.isVisible = true
                    replyEdit.setImageResource(R.drawable.ic_close)
                }
            }
        }
        reviewSaveBtn.setOnClickListener{
            val req = ReviewUpdateRequest(review.id, reviewEditStars.rating.roundToInt(), reviewEditText.text.toString())
            lifecycleScope.launch(Dispatchers.IO) {
                reviewsService.updateReview(req) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            Toast.makeText(requireContext(), "Review Updated", Toast.LENGTH_LONG).show()
                            refreshReviews()
                        } else {
                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }
        reviewDeleteBtn.setOnClickListener{
            lifecycleScope.launch(Dispatchers.IO) {
                reviewsService.deleteReview(review.id) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            Toast.makeText(requireContext(), "Review Deleted", Toast.LENGTH_LONG).show()
                            parentFragmentManager.beginTransaction()
                                .hide(this@ReviewFragment)
                                .commit()
                        } else {
                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }
        responseSaveBtn.setOnClickListener{
            if (review.response == null) {
                val req = ReviewResponseRequest(review.id, businessId, responseEditText.text.toString())
                lifecycleScope.launch(Dispatchers.IO) {
                    reviewsService.postResponse(req) { data ->
                        lifecycleScope.launch(Dispatchers.Main) {
                            if (data) {
                                Toast.makeText(requireContext(), "Response Posted", Toast.LENGTH_LONG).show()
                                refreshReviews()
                            } else {
                                Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                            }
                        }
                    }
                }
            } else {
                val req = ReviewResponseRequest(review.id, businessId, responseEditText.text.toString())
                lifecycleScope.launch(Dispatchers.IO) {
                    reviewsService.updateResponse(req) { data ->
                        lifecycleScope.launch(Dispatchers.Main) {
                            if (data) {
                                Toast.makeText(requireContext(), "Response Updated", Toast.LENGTH_LONG).show()
                                refreshReviews()
                            } else {
                                Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                            }
                        }
                    }
                }
            }

        }
        responseDeleteBtn.setOnClickListener{
            lifecycleScope.launch(Dispatchers.IO) {
                reviewsService.deleteResponse(review.id) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            Toast.makeText(requireContext(), "Response Deleted", Toast.LENGTH_LONG).show()
                            responseLayout.isVisible = false
                        } else {
                            Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                        }
                    }
                }
            }
        }

        return view
    }

    private fun editIsOpen(drawable: Drawable): Boolean {
        val closeDrawable = ContextCompat.getDrawable(requireContext(), R.drawable.ic_close)

        if (drawable is BitmapDrawable && closeDrawable is BitmapDrawable) {
            val bitmap1 = drawable.bitmap
            val bitmap2 = closeDrawable.bitmap
            return bitmap1.sameAs(bitmap2)
        }
        return false
    }

    private fun initView() {
        if (!businessBelongsToUser) {
            replyEdit.setImageResource(R.drawable.ic_pencil)
        }
        if (review.response == null) {
            responseLayout.isVisible = false
        }
    }

    private fun refreshReviews() {
        (parentFragment as? ReviewsWrapperFragment)?.addReviews()
    }
}