package com.example.statstrack.fragments.homepages.searchpage

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.example.statstrack.R
import com.google.android.material.bottomsheet.BottomSheetDialogFragment

class SearchWrapperBusinessesFragment : BottomSheetDialogFragment() {

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val view = inflater.inflate(R.layout.fragment_search_wrapper_businesses, container, false)

        return view
    }

}