package com.example.statstrack.fragments.homepages.searchpage

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.Toast
import com.example.statstrack.R
import com.example.statstrack.fragments.common.BusinessFragment
import com.example.statstrack.fragments.homepages.SearchPageFragment
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import java.util.UUID

class SearchWrapperBusinessesFragment : BottomSheetDialogFragment() {

    private lateinit var layout: LinearLayout

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val view = inflater.inflate(R.layout.fragment_search_wrapper_businesses, container, false)

        layout = view.findViewById(R.id.searchWrapperBusinessesLayout)

        initBusinesses()

        return view
    }

    private fun getBusinesses(): List<BusinessResponse>{
        val parentFragment = parentFragment
        if(parentFragment is SearchPageFragment)
            return parentFragment.getBusinesses()
        return listOf()
    }

    private fun initBusinesses() {
        val businesses = getBusinesses()
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()
        layout.removeAllViews()
        if(businesses.isEmpty())
            Toast.makeText(requireContext(), "No businesses where found", Toast.LENGTH_LONG).show()
        for (business in businesses) {
            val fragment = BusinessFragment(business)
            fragmentTransaction.add(layout.id, fragment, business.id.toString())
        }
        fragmentTransaction.commit()
    }

}