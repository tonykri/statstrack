package com.example.statstrack.fragments.homepages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.fragments.common.BusinessFragment
import com.example.statstrack.fragments.common.CouponFragment
import com.example.statstrack.helper.apiCalls.HomePageService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class CouponsPageFragment : Fragment() {

    private lateinit var layout: LinearLayout

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
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_coupons_page, container, false)

        layout = view.findViewById(R.id.couponsPageFragmentLayout)
        addCoupons()

        return view
    }

    private fun addCoupons() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        layout.removeAllViews()

        lifecycleScope.launch(Dispatchers.IO) {
            homePageService.getMyCoupons() { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        for (coupon in data) {
                            val fragment = CouponFragment(coupon)
                            fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment)
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