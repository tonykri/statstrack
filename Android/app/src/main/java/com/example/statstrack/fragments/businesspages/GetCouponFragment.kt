package com.example.statstrack.fragments.businesspages

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.BusinessService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class GetCouponFragment(private val businessId: UUID) : Fragment() {

    private lateinit var getCouponBtn: Button
    private val businessService: BusinessService by lazy {
        BusinessService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_get_coupon, container, false)

        getCouponBtn = view.findViewById(R.id.getCouponFragmentBtn)

        getCouponBtn.setOnClickListener{
            getCoupon()
        }


        return view
    }

    private fun getCoupon() {
        getCouponBtn.isEnabled = false
        lifecycleScope.launch(Dispatchers.IO) {
            businessService.getCoupon(businessId) { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data) {
                        Toast.makeText(requireContext(), "Coupon created", Toast.LENGTH_LONG).show()
                    } else {
                        Toast.makeText(requireContext(), "Could not get a coupon", Toast.LENGTH_LONG).show()
                    }
                    getCouponBtn.isEnabled = true
                }
            }
        }
    }

}