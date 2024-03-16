package com.example.statstrack.fragments.businesspages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.LinearLayout
import android.widget.Spinner
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.editpage.PhotoEditFragment
import com.example.statstrack.fragments.businesspages.reviewspage.AddReviewFragment
import com.example.statstrack.fragments.businesspages.reviewspage.ReviewFragment

class BusinessEditFragment : Fragment() {

    private lateinit var photosLayout: LinearLayout
    private lateinit var categoriesSpinner: Spinner

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_business_edit, container, false)

        photosLayout = view.findViewById(R.id.businessEditFragmentPhotosLayout)
        categoriesSpinner = view.findViewById(R.id.businessEditFragmentSpinner)
        initPhotos()
        initSpinner()

        return view
    }

    private fun initPhotos() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        photosLayout.removeAllViews()

        val fragment1 = PhotoEditFragment()
        val fragment2 = PhotoEditFragment()
        val fragment3 = PhotoEditFragment()
        val fragment4 = PhotoEditFragment()

        fragmentTransaction.add(photosLayout.id, fragment1)
        fragmentTransaction.add(photosLayout.id, fragment2)
        fragmentTransaction.add(photosLayout.id, fragment3)
        fragmentTransaction.add(photosLayout.id, fragment4)

        fragmentTransaction.commit()
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