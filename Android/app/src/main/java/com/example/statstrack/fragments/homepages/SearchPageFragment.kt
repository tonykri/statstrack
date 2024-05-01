package com.example.statstrack.fragments.homepages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.common.BusinessFragment
import com.example.statstrack.fragments.homepages.searchpage.CategoriesNavbarFragment
import com.example.statstrack.fragments.homepages.searchpage.MapsFragment
import com.example.statstrack.fragments.homepages.searchpage.SearchWrapperBusinessesFragment
import com.example.statstrack.helper.apiCalls.HomePageService
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.google.android.gms.maps.model.LatLng
import com.google.android.material.bottomsheet.BottomSheetBehavior
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class SearchPageFragment : Fragment() {

    private var businesses: List<BusinessResponse> = listOf()
    private var category: String? = null
    private lateinit var topRightLatLng: LatLng
    private lateinit var bottomLeftLatLng: LatLng
    private val mapsFragment = MapsFragment()

    private val homePageService: HomePageService by lazy {
        HomePageService(requireContext())
    }

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
            .replace(R.id.searchPageFragmentMapLayout, mapsFragment)
            .commit()

        val bottomSheetFragment = SearchWrapperBusinessesFragment()
        val bottomSheet: View = view.findViewById(R.id.searchPageFragmentSwipeLayout)
        val bottomSheetBehavior = BottomSheetBehavior.from(bottomSheet)

        bottomSheetBehavior.addBottomSheetCallback(object : BottomSheetBehavior.BottomSheetCallback() {
            override fun onStateChanged(bottomSheet: View, newState: Int) {
                when (newState) {
                    BottomSheetBehavior.STATE_EXPANDED -> {
                        bottomSheetFragment.show(childFragmentManager, bottomSheetFragment.tag)
                    }
                    BottomSheetBehavior.STATE_COLLAPSED -> {
                        bottomSheetFragment.dismiss()
                    }
                    else -> {  }
                }
            }
            override fun onSlide(bottomSheet: View, slideOffset: Float) {
            }
        })


        return view
    }

    fun getCategory(category: String? = null) {
        this.category = category
        lifecycleScope.launch(Dispatchers.IO) {
            homePageService.getBusinesses(topRightLatLng, bottomLeftLatLng, category) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        businesses = data
                        mapsFragment.updateMap()
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

    fun storeLatLng(topRightLatLng: LatLng, bottomLeftLatLng: LatLng, callback: () -> Unit) {
        this.topRightLatLng = topRightLatLng
        this.bottomLeftLatLng = bottomLeftLatLng
        lifecycleScope.launch(Dispatchers.IO) {
            homePageService.getBusinesses(topRightLatLng, bottomLeftLatLng, category) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        businesses = data
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
        callback()
    }

    fun getBusinesses(): List<BusinessResponse> {
        return businesses
    }
}