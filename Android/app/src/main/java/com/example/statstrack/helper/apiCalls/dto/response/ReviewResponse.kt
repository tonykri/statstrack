package com.example.statstrack.helper.apiCalls.dto.response

import java.util.UUID

data class ReviewResponse(
    val id: UUID,
    val stars: Int,
    val content: String?,
    val lastModified: String,
    val response: ReviewBusinessResponse?
)

data class ReviewBusinessResponse(
    val content: String?,
    val lastModified: String,
)