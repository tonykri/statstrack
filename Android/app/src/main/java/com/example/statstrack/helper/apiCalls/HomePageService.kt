package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Log
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.example.statstrack.helper.apiCalls.dto.response.CouponResponse
import com.example.statstrack.helper.apiCalls.dto.response.UserResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.result.Result
import com.google.gson.Gson
import java.util.UUID

class HomePageService(context: Context) {
    private val userBaseUrl = "http://10.0.2.2:4000"
    private val businessBaseUrl = "http://10.0.2.2:4001"
    private val couponBaseUrl = "http://10.0.2.2:4002"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    fun getMyBusinesses(callback: (List<BusinessResponse>?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/mybusinesses"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        val gson = Gson()
                        val businessResponse: List<BusinessResponse> = gson.fromJson(responseData, Array<BusinessResponse>::class.java).toList()

                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(businessResponse)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }
    }

    fun getMyCoupons(callback: (List<CouponResponse>?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$couponBaseUrl/coupon/all"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        val gson = Gson()
                        val coupons: List<CouponResponse> = gson.fromJson(responseData, Array<CouponResponse>::class.java).toList()

                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(coupons)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }

    }

    fun getUserData(callback: (UserResponse?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$userBaseUrl/profile/user"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        val gson = Gson()
                        val userResponse: UserResponse = gson.fromJson(responseData, UserResponse::class.java)

                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(userResponse)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }
    }

    fun getBusinessPhoto(businessId: UUID, photoId: UUID, callback: (Bitmap?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/photos/$businessId/$photoId"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = result.get()
                        val bitmap = BitmapFactory.decodeByteArray(responseData, 0, responseData.size)
                        callback(bitmap)

                        Log.d("SUCCESS: ", "Data retrieved")
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }
    }

}