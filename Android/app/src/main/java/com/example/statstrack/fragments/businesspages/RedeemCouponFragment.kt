package com.example.statstrack.fragments.businesspages

import android.app.AlertDialog
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.BusinessService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class RedeemCouponFragment(private val businessId: UUID) : Fragment() {

    private lateinit var redeemBtn: Button
    private lateinit var codeInput: EditText

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
        val view = inflater.inflate(R.layout.fragment_redeem_coupon, container, false)

        redeemBtn = view.findViewById(R.id.getCouponFragmentBtn)
        codeInput = view.findViewById(R.id.redeemCouponFragmentCode)

        redeemBtn.setOnClickListener{
            redeemBtn.isEnabled = false
            lifecycleScope.launch(Dispatchers.IO) {
                businessService.redeemCoupon(businessId, codeInput.text.toString()) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            Toast.makeText(requireContext(), "Coupon redeemed", Toast.LENGTH_LONG).show()
                        } else {
                            val builder = AlertDialog.Builder(context)
                            builder.setTitle("Not accepted")
                            builder.setMessage("The code you provided is not valid. Try another code")
                            builder.setPositiveButton("OK") { dialog, _ ->
                                dialog.dismiss()
                            }
                            val dialog = builder.create()
                            dialog.show()
                        }
                        redeemBtn.isEnabled = true
                    }
                }
            }
        }

        return view
    }

}