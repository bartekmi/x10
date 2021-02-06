// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Checkbox from 'latitude/Checkbox';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';

import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import VisibilityControl from 'react_lib/VisibilityControl';

import { companyEntityApplicableWhenForPhysicalAddress, type CompanyEntity } from 'client_page/entities/CompanyEntity';
import Address from 'client_page/ui/Address';

import { type ClientViewCompanyEntity_companyEntity } from './__generated__/ClientViewCompanyEntity_companyEntity.graphql';



type Props = {|
  +companyEntity: ClientViewCompanyEntity_companyEntity,
|};
function ClientViewCompanyEntity(props: Props): React.Node {
  const { companyEntity } = props;

  return (
    <Expander
      header={
        {
          justifyContent: 'space-between',
        }
      }
      body={
        {
          flexDirection: 'column',
        }
      }
    >
      <Group
        flexDirection='column'
      >
        <Group>
          <FloatInput
            value={ companyEntity.coreId }
            onChange={ () => { } }
            readOnly={ true }
          />
          <DisplayField
            label='Mailing Address'
          >
            <Address address={ companyEntity.mailingAddress }/>
          </DisplayField>
          <DisplayField
            label='Physical Address'
          >
            <VisibilityControl
              visible={ companyEntityApplicableWhenForPhysicalAddress(companyEntity) }
            >
              <Address address={ companyEntity.physicalAddress }/>
            </VisibilityControl>
          </DisplayField>
        </Group>
        <Group
          justifyContent='space-between'
        >
          <Group>
            <Checkbox
              checked={ companyEntity.isPrimary }
              onChange={ (value) => {
                onChange({ ...companyEntity, isPrimary: value })
              } }
              label='Primary Entity'
              disabled={ companyEntity.isPrimary }
            />
            <VisibilityControl
              visible={ companyEntity.isPrimary }
            >
              <HelpTooltip
                text='You can change the primary entity by selecting...'
              />
            </VisibilityControl>
          </Group>
        </Group>
      </Group>
    </Expander>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ClientViewCompanyEntity, {
  companyEntity: graphql`
    fragment ClientViewCompanyEntity_companyEntity on CompanyEntity {
      id
      companyType
      coreId
      isPrimary
      legalName
      mailingAddress {
        id
        ...Address_address
      }
      mailingAddressIsPhysicalAddress
      physicalAddress {
        id
        ...Address_address
      }
    }
  `,
});

