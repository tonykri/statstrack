package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.graphics.Bitmap
import android.util.Log
import com.example.statstrack.helper.apiCalls.dto.request.BusinessUpdateRequest
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.fuel.core.FileDataPart
import com.github.kittinunf.fuel.core.Method
import com.github.kittinunf.fuel.core.extensions.jsonBody
import com.github.kittinunf.result.Result
import com.google.gson.Gson
import java.io.File
import java.io.FileOutputStream
import java.io.IOException
import java.util.UUID

class BusinessService(context: Context) {
    private val businessBaseUrl = "http://10.0.2.2:4001"
    private val couponBaseUrl = "http://10.0.2.2:4002"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    fun getBusinessPhotos(businessId: UUID?, callback: (List<UUID>?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/photos/$businessId"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        Log.d("DATA: ", responseData)
                        val gson = Gson()
                        val photos: List<UUID> = gson.fromJson(responseData, Array<UUID>::class.java).toList()

                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(photos)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }
    }

    fun getBusiness(businessId: UUID?, callback: (BusinessResponse?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/$businessId"
        Log.d("BID", businessId.toString())
        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        Log.d("DATA: ", responseData)
                        val gson = Gson()
                        val business: BusinessResponse = gson.fromJson(responseData, BusinessResponse::class.java)

                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(business)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", result.toString())
                        callback(null)
                    }
                }
            }
    }

    fun getCoupon(businessId: UUID, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$couponBaseUrl/coupon/$businessId"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(false)
                    }
                }
            }
    }

    fun redeemCoupon(businessId: UUID, code: String, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$couponBaseUrl/coupon/redeem/$businessId/$code"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(false)
                    }
                }
            }
    }

    fun updateBusiness(business: BusinessUpdateRequest, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/update"

        val jsonBody = """
        {
            "BID": "${business.id}",
            "Brand": "${business.brand}",
            "Description": "${business.description}",
            "Category": "${business.category}",
            "Address": "${business.address}",
            "Latitude": ${business.latitude},
            "Longitude": ${business.longitude}
        }
        """.trimIndent()

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(false)
                    }
                }
            }
    }

    fun deleteBusinessPhoto(photoId: UUID, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/photos/$photoId"

        Fuel.delete(url)
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(false)
                    }
                }
            }
    }

    private fun bitmapToFile(context: Context, bitmap: Bitmap, fileName: String): File {
        val file = File(context.cacheDir, fileName)
        try {
            val fileOutputStream = FileOutputStream(file)
            bitmap.compress(Bitmap.CompressFormat.JPEG, 100, fileOutputStream)
            fileOutputStream.flush()
            fileOutputStream.close()
        } catch (e: IOException) {
            e.printStackTrace()
        }
        return file
    }


    fun addBusinessPhoto(context: Context, businessId: UUID, photo: Bitmap, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$businessBaseUrl/photos/${businessId}"

        val file = bitmapToFile(context, photo, "photo.jpg")

        Fuel.upload(url, Method.POST)
            .add(FileDataPart(file, name = "photos", filename = "photo.jpg"))
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(false)
                    }
                }
            }
    }
}