package com.example.statstrack.helper.apiCalls.dto.response
import java.util.UUID

data class AccountResponse(
    val id: UUID,
    var firstName: String?,
    var lastName: String?,
    var email: String?,
    var provider: String?,
    var profileStage: String?,
    var accessToken: String?,
    var refreshToken: String?
)
