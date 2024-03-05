package com.example.statstrack.fragments.common

import android.graphics.Color
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.core.graphics.drawable.DrawableCompat
import androidx.core.widget.TextViewCompat
import com.example.statstrack.R
import com.example.statstrack.activities.HomeActivity
import kotlin.properties.Delegates

class NavbarFragment : Fragment() {

    private lateinit var searchBtn: TextView
    private lateinit var myBusinessesBtn: TextView
    private lateinit var couponsBtn: TextView
    private lateinit var profileBtn: TextView

    private var tealDark by Delegates.notNull<Int>()
    private var black by Delegates.notNull<Int>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_navbar, container, false)

        searchBtn = view.findViewById(R.id.navbarFragmentSearch)
        myBusinessesBtn = view.findViewById(R.id.navbarFragmentMyBusinesses)
        couponsBtn = view.findViewById(R.id.navbarFragmentCoupons)
        profileBtn = view.findViewById(R.id.navbarFragmentProfile)

        tealDark = ContextCompat.getColor(requireContext(), R.color.tealDark)
        black = ContextCompat.getColor(requireContext(), R.color.black)

        setDrawableSearch()
        myBusinessesBtn.setOnClickListener{
            setDrawableBusiness()
            (requireActivity() as? HomeActivity)?.goToMyBusinessesPage()
        }
        searchBtn.setOnClickListener{
            setDrawableSearch()
            (requireActivity() as? HomeActivity)?.goToSearchPage()
        }
        couponsBtn.setOnClickListener{
            setDrawableCoupon()
        }
        profileBtn.setOnClickListener{
            setDrawableProfile()
        }

        return view
    }

    private fun setDrawableSearch() {
        myBusinessesBtn.setTextColor(black)
        val icBusiness = ContextCompat.getDrawable(requireContext(), R.drawable.ic_business)
        if (icBusiness != null) {
            val tintedDrawable = DrawableCompat.wrap(icBusiness.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(myBusinessesBtn, null, tintedDrawable, null, null)
        }

        searchBtn.setTextColor(tealDark)
        val icSearch = ContextCompat.getDrawable(requireContext(), R.drawable.ic_search)
        if (icSearch != null) {
            val tintedDrawable = DrawableCompat.wrap(icSearch.mutate())
            DrawableCompat.setTint(tintedDrawable, tealDark)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(searchBtn, null, tintedDrawable, null, null)
        }

        couponsBtn.setTextColor(black)
        val icCoupon = ContextCompat.getDrawable(requireContext(), R.drawable.ic_coupon)
        if (icCoupon != null) {
            val tintedDrawable = DrawableCompat.wrap(icCoupon.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(couponsBtn, null, tintedDrawable, null, null)
        }

        profileBtn.setTextColor(black)
        val icProfile = ContextCompat.getDrawable(requireContext(), R.drawable.ic_profile)
        if (icProfile != null) {
            val tintedDrawable = DrawableCompat.wrap(icProfile.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(profileBtn, null, tintedDrawable, null, null)
        }
    }
    private fun setDrawableBusiness() {
        myBusinessesBtn.setTextColor(tealDark)
        val icBusiness = ContextCompat.getDrawable(requireContext(), R.drawable.ic_business)
        if (icBusiness != null) {
            val tintedDrawable = DrawableCompat.wrap(icBusiness.mutate())
            DrawableCompat.setTint(tintedDrawable, tealDark)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(myBusinessesBtn, null, tintedDrawable, null, null)
        }

        searchBtn.setTextColor(black)
        val icSearch = ContextCompat.getDrawable(requireContext(), R.drawable.ic_search)
        if (icSearch != null) {
            val tintedDrawable = DrawableCompat.wrap(icSearch.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(searchBtn, null, tintedDrawable, null, null)
        }

        couponsBtn.setTextColor(black)
        val icCoupon = ContextCompat.getDrawable(requireContext(), R.drawable.ic_coupon)
        if (icCoupon != null) {
            val tintedDrawable = DrawableCompat.wrap(icCoupon.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(couponsBtn, null, tintedDrawable, null, null)
        }

        profileBtn.setTextColor(black)
        val icProfile = ContextCompat.getDrawable(requireContext(), R.drawable.ic_profile)
        if (icProfile != null) {
            val tintedDrawable = DrawableCompat.wrap(icProfile.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(profileBtn, null, tintedDrawable, null, null)
        }
    }
    private fun setDrawableCoupon() {
        myBusinessesBtn.setTextColor(black)
        val icBusiness = ContextCompat.getDrawable(requireContext(), R.drawable.ic_business)
        if (icBusiness != null) {
            val tintedDrawable = DrawableCompat.wrap(icBusiness.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(myBusinessesBtn, null, tintedDrawable, null, null)
        }

        searchBtn.setTextColor(black)
        val icSearch = ContextCompat.getDrawable(requireContext(), R.drawable.ic_search)
        if (icSearch != null) {
            val tintedDrawable = DrawableCompat.wrap(icSearch.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(searchBtn, null, tintedDrawable, null, null)
        }

        couponsBtn.setTextColor(tealDark)
        val icCoupon = ContextCompat.getDrawable(requireContext(), R.drawable.ic_coupon)
        if (icCoupon != null) {
            val tintedDrawable = DrawableCompat.wrap(icCoupon.mutate())
            DrawableCompat.setTint(tintedDrawable, tealDark)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(couponsBtn, null, tintedDrawable, null, null)
        }

        profileBtn.setTextColor(black)
        val icProfile = ContextCompat.getDrawable(requireContext(), R.drawable.ic_profile)
        if (icProfile != null) {
            val tintedDrawable = DrawableCompat.wrap(icProfile.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(profileBtn, null, tintedDrawable, null, null)
        }
    }
    private fun setDrawableProfile() {
        myBusinessesBtn.setTextColor(black)
        val icBusiness = ContextCompat.getDrawable(requireContext(), R.drawable.ic_business)
        if (icBusiness != null) {
            val tintedDrawable = DrawableCompat.wrap(icBusiness.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(myBusinessesBtn, null, tintedDrawable, null, null)
        }

        searchBtn.setTextColor(black)
        val icSearch = ContextCompat.getDrawable(requireContext(), R.drawable.ic_search)
        if (icSearch != null) {
            val tintedDrawable = DrawableCompat.wrap(icSearch.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(searchBtn, null, tintedDrawable, null, null)
        }

        couponsBtn.setTextColor(black)
        val icCoupon = ContextCompat.getDrawable(requireContext(), R.drawable.ic_coupon)
        if (icCoupon != null) {
            val tintedDrawable = DrawableCompat.wrap(icCoupon.mutate())
            DrawableCompat.setTint(tintedDrawable, black)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(couponsBtn, null, tintedDrawable, null, null)
        }

        profileBtn.setTextColor(tealDark)
        val icProfile = ContextCompat.getDrawable(requireContext(), R.drawable.ic_profile)
        if (icProfile != null) {
            val tintedDrawable = DrawableCompat.wrap(icProfile.mutate())
            DrawableCompat.setTint(tintedDrawable, tealDark)
            TextViewCompat.setCompoundDrawablesRelativeWithIntrinsicBounds(profileBtn, null, tintedDrawable, null, null)
        }
    }

}