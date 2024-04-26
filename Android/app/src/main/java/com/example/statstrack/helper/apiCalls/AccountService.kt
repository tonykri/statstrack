package com.example.statstrack.helper.apiCalls
import android.annotation.SuppressLint
import android.content.Context
import android.util.Log
import android.widget.Toast
import com.example.statstrack.helper.apiCalls.dto.response.AccountResponse
import com.github.kittinunf.fuel.Fuel
import com.github.kittinunf.fuel.core.extensions.jsonBody
import com.github.kittinunf.result.Result
import com.google.gson.Gson
import java.util.UUID

class AccountService(context: Context) {
    private val baseUrl = "http://10.0.2.2:4007"
    private val sharedPref = context.getSharedPreferences("UserData", Context.MODE_PRIVATE)

    @SuppressLint("CommitPrefEdits")
    private fun storeData(data: AccountResponse?) {
        if (data == null)
            return

        val editor = sharedPref?.edit()
        editor?.clear()
        editor?.putString("id", data.id.toString())
        editor?.putString("firstName", data.firstName)
        editor?.putString("lastName", data.lastName)
        editor?.putString("email", data.email)
        editor?.putString("provider", data.provider)
        editor?.putString("profileStage", data.profileStage)
        editor?.putString("accessToken", data.accessToken)
        editor?.putString("refreshToken", data.refreshToken)
        editor?.apply()
    }

    fun login(email: String, callback: (Boolean) -> Unit) {
        val url = "$baseUrl/login/$email"

        Fuel.get(url)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        Log.d("SUCCESS: ", "Email Sent")
                        callback(true)
                    }
                    is Result.Failure -> {
                        val err = result.error
                        Log.d("ERROR: ", "${err.message}", err)
                        callback(false)
                    }
                }
            }
    }

    fun login(email: String, code: String, callback: (Boolean) -> Unit) {
        val url = "$baseUrl/login"
        val jsonBody = """
        {
            "Email": "$email",
            "Code": "$code"
        }
        """.trimIndent()
        var myData: AccountResponse

        Fuel.post(url)
            .jsonBody(jsonBody)
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        val gson = Gson()
                        myData = gson.fromJson(responseData, AccountResponse::class.java)
                        Log.d("SUCCESS: ", "Data retrieved")
                        storeData(myData)
                        callback(true)
                    }
                    is Result.Failure -> {
                        val err = result.error
                        Log.d("ERROR: ", "${err.message}", err)
                        callback(false)
                    }
                }
            }
    }

    fun register(email: String, firstname: String, lastname: String, callback: (Boolean) -> Unit) {
        val url = "$baseUrl/register"
        val jsonBody = """
        {
            "Email": "$email",
            "FirstName": "$firstname",
            "LastName": "$lastname"
        }
        """.trimIndent()

        Fuel.post(url)
            .jsonBody(jsonBody)
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        callback(true)
                    }
                    is Result.Failure -> {
                        callback(false)
                    }
                }
            }
    }

    fun delete(): Int {
        val token = sharedPref.getString("accessToken", "")

        val url = "$baseUrl/delete"
        var success: Boolean = false

        Fuel.get(url)
            .header("Authorization" to "Bearer $token")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        success = true
                    }
                    is Result.Failure -> {
                    }
                }
            }
        return if (success) 0 else 1
    }

    fun delete(code: String): Int {
        val token = sharedPref.getString("accessToken", "")

        val url = "$baseUrl/delete"
        var success: Boolean = false

        Fuel.delete(url)
            .header("Authorization" to "Bearer $token")
            .header("code" to "$code")
            .response { _, _, result ->
                when (result) {
                    is Result.Success -> {
                        success = true
                    }
                    is Result.Failure -> {
                    }
                }
            }
        return if (success) 0 else 1
    }

    fun refreshToken(callback: (Boolean) -> Unit) {
        val token = sharedPref.getString("refreshToken", "")

        val url = "$baseUrl/refreshtoken"
        var myData: AccountResponse

        Fuel.get(url)
            .header("token" to "$token")
            .response { _, response, result ->
                when (result) {
                    is Result.Success -> {
                        val responseData = String(response.data)
                        val gson = Gson()
                        myData = gson.fromJson(responseData, AccountResponse::class.java)
                        Log.d("SUCCESS: ", "Data retrieved")
                        storeData(myData)
                        callback(true)
                    }
                    is Result.Failure -> {
                        Log.d("FAILED: ", "Data could not retrieved")
                        callback(false)
                    }
                }
            }
    }
}