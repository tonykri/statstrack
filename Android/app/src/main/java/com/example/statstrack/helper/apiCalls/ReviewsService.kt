package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.util.Log
import com.example.statstrack.helper.apiCalls.dto.request.ReviewCreateRequest
import com.example.statstrack.helper.apiCalls.dto.request.ReviewResponseRequest
import com.example.statstrack.helper.apiCalls.dto.request.ReviewUpdateRequest
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.example.statstrack.helper.apiCalls.dto.response.ReviewResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.fuel.core.extensions.jsonBody
import com.github.kittinunf.result.Result
import com.google.gson.Gson
import java.util.UUID

class ReviewsService(context: Context) {
    private val baseUrl = "http://10.0.2.2:4003"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    fun getReviews(businessId: UUID, callback: (List<ReviewResponse>?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl/$businessId"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        Log.d("DATA: ", responseData)
                        try {
                            val gson = Gson()
                            val data: List<ReviewResponse> = gson.fromJson(responseData, Array<ReviewResponse>::class.java).toList()
                            Log.d("SUCCESS: ", "Data retrieved")
                            callback(data)
                        }catch (e: Exception) {
                            Log.d("ERROR MESSAGE: ", e.message.toString())
                        }
                    }
                    is Result.Failure -> {
                        Log.d("ERROR: ", "Something is wrong")
                        callback(null)
                    }
                }
            }
    }

    fun postReview(data: ReviewCreateRequest, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl"
        val gson = Gson()
        val json = gson.toJson(data)

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(json)
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

    fun updateReview(data: ReviewUpdateRequest, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl"
        val gson = Gson()
        val json = gson.toJson(data)

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(json)
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

    fun deleteReview(reviewId: UUID, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl/$reviewId"

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

    fun postResponse(data: ReviewResponseRequest, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl/response"
        val gson = Gson()
        val json = gson.toJson(data)

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(json)
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

    fun updateResponse(data: ReviewResponseRequest, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl/response"
        val gson = Gson()
        val json = gson.toJson(data)

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(json)
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

    fun deleteResponse(reviewId: UUID, callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("accessToken", "")
        val url = "$baseUrl/response/$reviewId"

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

}