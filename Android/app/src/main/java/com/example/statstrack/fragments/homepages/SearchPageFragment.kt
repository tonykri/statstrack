package com.example.statstrack.fragments.homepages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.example.statstrack.R
import com.example.statstrack.fragments.homepages.searchpage.CategoriesNavbarFragment
import com.example.statstrack.fragments.homepages.searchpage.MapsFragment
import com.example.statstrack.fragments.homepages.searchpage.SearchWrapperBusinessesFragment
import com.google.android.material.bottomsheet.BottomSheetBehavior

class SearchPageFragment : Fragment() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_search_page, container, false)

        childFragmentManager.beginTransaction()
            .replace(R.id.searchPageFragmentCategoriesNavbarLayout, CategoriesNavbarFragment())
            .commit()
        childFragmentManager.beginTransaction()
            .replace(R.id.searchPageFragmentMapLayout, MapsFragment())
            .commit()

        val bottomSheetFragment = SearchWrapperBusinessesFragment()
        val bottomSheet: View = view.findViewById(R.id.searchPageFragmentSwipeLayout)
        val bottomSheetBehavior = BottomSheetBehavior.from(bottomSheet)

        bottomSheetBehavior.addBottomSheetCallback(object : BottomSheetBehavior.BottomSheetCallback() {
            override fun onStateChanged(bottomSheet: View, newState: Int) {
                when (newState) {
                    BottomSheetBehavior.STATE_EXPANDED -> {
                        //bottomSheetFragment.show(childFragmentManager, bottomSheetFragment.tag)
                    }
                    BottomSheetBehavior.STATE_COLLAPSED -> {
                        bottomSheetFragment.dismiss()
                    }

                    BottomSheetBehavior.STATE_DRAGGING -> {
                        bottomSheetFragment.show(childFragmentManager, bottomSheetFragment.tag)
                    }
                }
            }
            override fun onSlide(bottomSheet: View, slideOffset: Float) {
            }
        })

        return view
    }

}