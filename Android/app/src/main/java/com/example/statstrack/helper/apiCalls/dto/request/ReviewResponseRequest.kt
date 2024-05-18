package com.example.statstrack.helper.apiCalls.dto.request

import java.util.UUID

data class ReviewResponseRequest(
    val reviewId: UUID,
    val businessId: UUID,
    val content: String
) {
    init {
        require(content.length in 1..200) { "Content length must be between 1 and 200 characters." }
    }
}