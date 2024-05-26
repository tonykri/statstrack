package com.example.statstrack.helper.apiCalls.dto.request

import java.util.UUID

data class BusinessUpdateRequest(
    val id: UUID,
    val brand: String,
    val description: String,
    val category: String,
    val address: String,
    val latitude: Double,
    val longitude: Double
)