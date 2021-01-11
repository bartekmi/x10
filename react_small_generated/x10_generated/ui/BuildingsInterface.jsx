// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import { type Building } from 'entities/Building';
import environment from 'environment';
import * as React from 'react';
import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';
import { graphql, QueryRenderer } from 'react-relay';
import Buildings from 'ui/Buildings';


export default function BuildingsInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (buildings) => <Buildings buildings={ buildings }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query BuildingsInterfaceQuery($id: Int!) {
    entity: building(id: $id) {
      id
      dbid
      moniker
      name
      description
      dateOfOccupancy
      mailboxType
      petPolicy
      mailingAddressSameAsPhysical
      units {
        id
        dbid
        number
        squareFeet
        numberOfBedrooms
        numberOfBathrooms
        hasBalcony
      }
      physicalAddress {
        id
        dbid
        unitNumber
        theAddress
        city
        stateOrProvince
        zip
      }
      mailingAddress {
        id
        dbid
        unitNumber
        theAddress
        city
        stateOrProvince
        zip
      }
    }
  }
`;

