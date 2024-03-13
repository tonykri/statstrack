package com.example.statstrack.fragments.homepages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import com.example.statstrack.R
import com.example.statstrack.fragments.common.CouponFragment

class CouponsPageFragment : Fragment() {

    private lateinit var layout: LinearLayout

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

        val fragment1 = CouponFragment()
        val fragment2 = CouponFragment()
        val fragment3 = CouponFragment()
        val fragment4 = CouponFragment()
        val fragment5 = CouponFragment()
        val fragment6 = CouponFragment()
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment1)
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment2)
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment3)
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment4)
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment5)
        fragmentTransaction.add(R.id.couponsPageFragmentLayout, fragment6)

        fragmentTransaction.commit()
    }

}