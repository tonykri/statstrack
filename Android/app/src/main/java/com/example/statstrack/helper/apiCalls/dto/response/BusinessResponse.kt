package com.example.statstrack.helper.apiCalls.dto.response

import java.time.LocalDateTime
import java.util.UUID

data class BusinessResponse(
    val id: UUID,
    val userId: UUID,
    val brand: String?,
    val description: String?,
    val category: String?,
    val address: String?,
    val latitude: Double,
    val longitude: Double,
    val expirationDate: String,
    val stars: Double = 0.0,
    val reviews: Int = 0,
    val photos: List<Photo> = emptyList()
)

data class Photo(
    val photoId: UUID = UUID.randomUUID(),
    val businessId: UUID,
    val business: BusinessResponse? = null
)

