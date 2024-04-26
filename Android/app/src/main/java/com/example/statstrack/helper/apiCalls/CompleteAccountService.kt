package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.media.session.MediaSession.Token
import android.util.Log
import com.example.statstrack.helper.apiCalls.dto.response.AccountResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.fuel.core.extensions.jsonBody
import com.github.kittinunf.result.Result
import com.google.gson.Gson

class CompleteAccountService(context: Context) {
    private val baseUrl = "http://10.0.2.2:4000"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    private fun updateToken(token: String) {
        val editor = sharedPref?.edit()
        editor?.putString("refreshToken", token)
        editor?.apply()
    }

    fun setUserData(birthdate: String, givenDialingNumber: String, phoneNumber: String, gender: String, givenCountry: String, callback: (Boolean) -> Unit) {
        val country = givenCountry.replace(" ", "_")
        val dialingNumber = givenDialingNumber.substring(1)
        val url = "$baseUrl/profile/user"
        val token = sharedPref.getString("accessToken", "")
        val jsonBody = """
        {
                "Birthdate": "$birthdate",
                "PhoneNumber": "$phoneNumber",
                "DialingCode": "$dialingNumber",
                "Gender": "$gender",
                "Country": "$country"
        }
        """.trimIndent()

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        updateToken(responseData)
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun setProfessionalData(givenEduLevel: String, givenIndustry: String, givenIncome: String, givenHours: String, callback: (Boolean) -> Unit) {
        val eduLevel = givenEduLevel.replace(" ", "_")
        val industry = givenIndustry.replace(" ", "_")
        val income = givenIncome.replace(" ", "_")
        val workingHours = givenHours.replace(" ", "_")

        val url = "$baseUrl/profile/professionallife"
        val token = sharedPref.getString("accessToken", "")
        val jsonBody = """
        {
            "LevelOfEducation": "$eduLevel",
            "Industry": "$industry",
            "Income": "$income",
            "WorkingHours": "$workingHours"
        }
        """.trimIndent()

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        updateToken(responseData)
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun setPersonalData(married: Boolean, stayHome: Boolean, callback: (Boolean) -> Unit) {
        val url = "$baseUrl/profile/personallife"
        val token = sharedPref.getString("accessToken", "")
        val jsonBody = """
        {
            "StayHome": $stayHome,
            "Married": $married
        }
        """.trimIndent()

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        updateToken(responseData)
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun setHobbies(hobbies: MutableList<String>, callback: (Boolean) -> Unit) {
        val finalItems = mutableListOf<String>()

        for (hobby in hobbies)
            finalItems.add("\"${hobby.replace(" ", "_")}\"")

        if (finalItems.size == 0 || finalItems.size > 7)
            callback(false)

        val url = "$baseUrl/profile/hobbies"
        val token = sharedPref.getString("accessToken", "")
        val jsonArray = finalItems.joinToString(", ", "[", "]")
        val jsonBody = """
        {
            "Hobbies": $jsonArray
        }
        """.trimIndent()

        Log.d("ITEMS", jsonBody)

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        updateToken(responseData)
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun setExpenses(expenses: MutableList<String>, callback: (Boolean) -> Unit) {
        val finalItems = mutableListOf<String>()

        for (expense in expenses)
            finalItems.add("\"${expense.replace(" ", "_")}\"")

        if (finalItems.size == 0 || finalItems.size > 7)
            callback(false)

        val url = "$baseUrl/profile/expenses"
        val token = sharedPref.getString("accessToken", "")
        val jsonArray = finalItems.joinToString(", ", "[", "]")
        val jsonBody = """
        {
            "Expenses": $jsonArray
        }
        """.trimIndent()

        Fuel.post(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        updateToken(responseData)
                        Log.d("SUCCESS: ", "Data retrieved")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }
}