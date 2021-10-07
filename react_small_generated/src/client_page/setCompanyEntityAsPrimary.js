// @flow

import { graphql } from 'react-relay';

import basicCommitMutation from 'react_lib/relay/basicCommitMutation';

export default function setCompanyEntityAsPrimary(id: string) {
  // Commenting out, since there is no back-end for this
  // basicCommitMutation(mutation, { id });
}

// const mutation = graphql`
//   mutation setCompanyEntityAsPrimaryMutation(
//     $id: String!
//   ) {
//     setCompanyEntityAsPrimary(
//       companyEntityId: $id
//     ) {
//       id
//       isPrimary
//     }
//   }
// `;