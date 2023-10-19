// @flow
import { type CtpatReviewStatusEnum } from "client_page/entities/CtpatReview";
import { type StatusIntent } from "latitude/Status";

// Would use CtpatReviewStatusEnum for type, but con't due to "%future added value"
export default function ctpatReviewStatusToText(status: ?string): string {
  switch (status) {
    case "COMPLIANT": return "Compliant";
    case "GRACE_PERIOD": return "Compliant (Grace Period)";
    case "NON_COMPLIANT": return "Non-Compliant";
    case "PROVISIONAL": return "Temporary Approval";
  }

  return status || "";
}