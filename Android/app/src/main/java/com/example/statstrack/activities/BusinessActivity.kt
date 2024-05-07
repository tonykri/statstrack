package com.example.statstrack.activities

import android.content.Context
import android.content.SharedPreferences
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.core.view.isVisible
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import androidx.lifecycle.lifecycleScope
import androidx.viewpager2.widget.ViewPager2
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.BusinessEditFragment
import com.example.statstrack.fragments.businesspages.BusinessInfoFragment
import com.example.statstrack.fragments.businesspages.GetCouponFragment
import com.example.statstrack.fragments.businesspages.RedeemCouponFragment
import com.example.statstrack.fragments.businesspages.ReviewsWrapperFragment
import com.example.statstrack.fragments.common.CouponFragment
import com.example.statstrack.helper.InitSettings
import com.example.statstrack.helper.SliderPagerAdapter
import com.example.statstrack.helper.apiCalls.BusinessService
import com.example.statstrack.helper.apiCalls.HomePageService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class BusinessActivity : AppCompatActivity() {

    private lateinit var layout: FrameLayout

    private lateinit var infoBtn: TextView
    private lateinit var reviewsBtn: TextView
    private lateinit var couponsSubBtn: TextView
    private lateinit var couponsBusBtn: TextView
    private lateinit var editBtn: TextView
    private lateinit var statsBtn: TextView
    private lateinit var viewPager: ViewPager2
    private val businessId = intent.getStringExtra("businessId")
    private val businessOwnerId = intent.getStringExtra("userId")

    private lateinit var sharedPref: SharedPreferences

    private val homePageService: HomePageService by lazy {
        HomePageService(this)
    }
    private val businessService: BusinessService by lazy {
        BusinessService(this)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_business)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()
        sharedPref = this.getSharedPreferences("UserData", Context.MODE_PRIVATE)

        viewPager = findViewById(R.id.businessActivityViewPager)

        layout = findViewById(R.id.businessActivityLayout)

        infoBtn = findViewById(R.id.businessActivityInfo)
        reviewsBtn = findViewById(R.id.businessActivityReviews)
        couponsSubBtn = findViewById(R.id.businessActivityCouponsSub)
        couponsBusBtn = findViewById(R.id.businessActivityCouponsBus)
        editBtn = findViewById(R.id.businessActivityEdit)
        statsBtn = findViewById(R.id.businessActivityStats)

        showButtons()
        replaceFragment(BusinessInfoFragment(UUID.fromString(businessId)))
        initPhotos(UUID.fromString(businessId))

        infoBtn.setOnClickListener{
            resetButtons()
            replaceFragment(BusinessInfoFragment(UUID.fromString(businessId)))
            infoBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        reviewsBtn.setOnClickListener{
            resetButtons()
            replaceFragment(ReviewsWrapperFragment())
            reviewsBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        couponsSubBtn.setOnClickListener{
            resetButtons()
            replaceFragment(GetCouponFragment(UUID.fromString(businessId)))
            couponsSubBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        couponsBusBtn.setOnClickListener{
            resetButtons()
            replaceFragment(RedeemCouponFragment(UUID.fromString(businessId)))
            couponsBusBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        editBtn.setOnClickListener{
            resetButtons()
            replaceFragment(BusinessEditFragment())
            editBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
    }

    private fun showButtons() {
        val userId = sharedPref.getString("id", "")
        if (userId.equals(businessOwnerId)) {
            couponsSubBtn.isVisible = false
        } else {
            couponsBusBtn.isVisible = false
            editBtn.isVisible = false
            statsBtn.isVisible = false
        }
    }

    private fun replaceFragment(fragment: Fragment) {
        val transaction: FragmentTransaction = supportFragmentManager.beginTransaction()
        transaction.replace(layout.id, fragment)
        transaction.commit()
    }

    private fun resetButtons() {
        infoBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
        reviewsBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
        couponsSubBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
        couponsBusBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
        editBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
        statsBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
    }

    private fun initPhotos(businessId: UUID) {
        val images: MutableList<Bitmap> = mutableListOf()

        lifecycleScope.launch(Dispatchers.IO) {
            businessService.getBusinessPhotos(businessId) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (!data.isNullOrEmpty()) {
                        for (photoId in data) {
                            lifecycleScope.launch(Dispatchers.IO) {
                                homePageService.getBusinessPhoto(businessId, photoId) { data ->
                                    lifecycleScope.launch(Dispatchers.Main) {
                                        if (data != null) {
                                            images.add(data)
                                        }
                                    }
                                }
                            }
                        }
                    } else {
                        val drawableId = R.drawable.no_image
                        val bitmap = BitmapFactory.decodeResource(resources, drawableId)
                        images.add(bitmap)
                    }
                }
            }
        }

        val adapter = SliderPagerAdapter(this, images)
        viewPager.adapter = adapter
    }
}
