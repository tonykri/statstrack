package com.example.statstrack.helper.apiCalls

import android.content.Context
import android.util.Log
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.fuel.core.extensions.jsonBody
import com.github.kittinunf.result.Result

class UpdateAccountService(context: Context) {
    private val baseUrl = "http://10.0.2.2:4000"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    fun updateHobbies(hobbies: MutableList<String>, callback: (Boolean) -> Unit) {
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

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Hobbies updated")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun updateExpenses(expenses: MutableList<String>, callback: (Boolean) -> Unit) {
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

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Expenses updated")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun updatePersonalData(married: Boolean, stayHome: Boolean, callback: (Boolean) -> Unit) {
        val url = "$baseUrl/profile/personallife"
        val token = sharedPref.getString("accessToken", "")
        val jsonBody = """
        {
            "StayHome": $stayHome,
            "Married": $married
        }
        """.trimIndent()

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Personal  data updated")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun updateProfessionalData(givenEduLevel: String, givenIndustry: String, givenIncome: String, givenHours: String, callback: (Boolean) -> Unit) {
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

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Professional date updated")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun updateUserData(birthdate: String, givenDialingNumber: String, phoneNumber: String, gender: String, givenCountry: String, callback: (Boolean) -> Unit) {
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

        Fuel.put(url)
            .header("Authorization" to "Bearer $token")
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "User data updated")
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }


}