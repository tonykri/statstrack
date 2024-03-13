package com.example.statstrack.activities

import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import androidx.viewpager2.widget.ViewPager2
import com.example.statstrack.R
import com.example.statstrack.fragments.businesspages.BusinessInfoFragment
import com.example.statstrack.fragments.businesspages.GetCouponFragment
import com.example.statstrack.fragments.businesspages.ReviewsWrapperFragment
import com.example.statstrack.helper.InitSettings
import com.example.statstrack.helper.SliderPagerAdapter

class BusinessActivity : AppCompatActivity() {

    private lateinit var layout: FrameLayout

    private lateinit var infoBtn: TextView
    private lateinit var reviewsBtn: TextView
    private lateinit var couponsSubBtn: TextView
    private lateinit var editBtn: TextView

    private val images = listOf(
        R.drawable.ic_business,
        R.drawable.ic_coupon,
        R.drawable.ic_google
    )
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_business)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        val viewPager: ViewPager2 = findViewById(R.id.businessActivityViewPager)
        val adapter = SliderPagerAdapter(this, images)
        viewPager.adapter = adapter

        layout = findViewById(R.id.businessActivityLayout)

        infoBtn = findViewById(R.id.businessActivityInfo)
        reviewsBtn = findViewById(R.id.businessActivityReviews)
        couponsSubBtn = findViewById(R.id.businessActivityCouponsSub)
        editBtn = findViewById(R.id.businessActivityEdit)

        replaceFragment(BusinessInfoFragment())

        infoBtn.setOnClickListener{
            resetButtons()
            replaceFragment(BusinessInfoFragment())
            infoBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        reviewsBtn.setOnClickListener{
            resetButtons()
            replaceFragment(ReviewsWrapperFragment())
            reviewsBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        couponsSubBtn.setOnClickListener{
            resetButtons()
            replaceFragment(GetCouponFragment())
            couponsSubBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
        }
        editBtn.setOnClickListener{
            resetButtons()
            editBtn.setTextColor(ContextCompat.getColor(this, R.color.orangeDark))
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
        editBtn.setTextColor(ContextCompat.getColor(this, R.color.black))
    }
}
