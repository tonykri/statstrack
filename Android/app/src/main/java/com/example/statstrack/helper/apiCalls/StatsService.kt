package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.util.Log
import com.example.statstrack.helper.apiCalls.dto.response.ReviewResponse
import com.example.statstrack.helper.apiCalls.dto.response.StatsResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.result.Result
import com.google.gson.Gson
import java.net.URLEncoder
import java.nio.charset.StandardCharsets
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.UUID

class StatsService(context: Context) {
    private val statsBaseUrl = "http://10.0.2.2:4008"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    fun getBusinessStats(businessId: UUID, startTime: LocalDateTime, endTime: LocalDateTime, callback: (StatsResponse?) -> Unit) {
        val token = sharedPref.getString("accessToken", "")

        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss")
        val formattedStartTime = URLEncoder.encode(startTime.format(formatter), StandardCharsets.UTF_8.toString())
        val formattedEndTime = URLEncoder.encode(endTime.format(formatter), StandardCharsets.UTF_8.toString())

        val url = "$statsBaseUrl/businessstats?businessId=$businessId&startTime=$formattedStartTime&endTime=$formattedEndTime"

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, response, result ->
                Log.d("res", response.toString())
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        Log.d("DATA: ", responseData)
                        try {
                            val gson = Gson()
                            val data: StatsResponse = gson.fromJson(responseData, StatsResponse::class.java)
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
}
