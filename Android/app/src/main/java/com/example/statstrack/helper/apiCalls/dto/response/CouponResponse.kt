package com.example.statstrack.helper.apiCalls.dto.response

import java.time.LocalDateTime
import java.util.UUID

data class CouponResponse(
    val businessId: UUID? = null,
    val brand: String? = null,
    var code: String = "",
    var purchaseDate: LocalDateTime,
    var redeemDate: LocalDateTime? = null

)
