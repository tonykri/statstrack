package com.example.statstrack.fragments.common

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.dto.response.CouponResponse

class CouponFragment(private val coupon: CouponResponse) : Fragment() {

    private lateinit var business: TextView
    private lateinit var code: TextView
    private lateinit var purchase: TextView
    private lateinit var redeem: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_coupon, container, false)

        business = view.findViewById(R.id.couponFragmentBrand)
        code = view.findViewById(R.id.couponFragmentCode)
        purchase = view.findViewById(R.id.couponFragmentPurchase)
        redeem = view.findViewById(R.id.couponFragmentRedeem)

        initData()

        return view
    }

    private fun initData() {
        business.text = coupon.brand
        code.text = coupon.code
        purchase.text = coupon.purchaseDate.toString()
        if (coupon.redeemDate != null)
            redeem.text = coupon.redeemDate.toString()
    }

}