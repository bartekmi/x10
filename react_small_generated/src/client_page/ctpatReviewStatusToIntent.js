// @flow
import { type CtpatReviewStatusEnum } from "client_page/entities/CtpatReview";
import { type StatusIntent } from "latitude/Status";

// Would use CtpatReviewStatusEnum for type, but con't due to "%future added value"
export default function ctpatReviewStatusToIntent(status: ?string): ?StatusIntent {
  switch (status) {
    case "COMPLIANT": return "complete";
    case "GRACE_PERIOD": return "due-soon";
    case "NON_COMPLIANT": return "error";
    case "PROVISIONAL": return "active";
  }

  return null;
}