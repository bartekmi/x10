// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Checkbox from 'latitude/Checkbox';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';
import SelectInput from 'latitude/select/SelectInput';

import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import Button from 'react_lib/latitude_wrappers/Button';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import VisibilityControl from 'react_lib/VisibilityControl';

import { companyEntityApplicableWhenForPhysicalAddress, CompanyEntityTypeEnumPairs, type CompanyEntity } from 'client_page/entities/CompanyEntity';
import setCompanyEntityAsPrimary from 'client_page/setCompanyEntityAsPrimary';
import AddressDisplay from 'client_page/ui/AddressDisplay';

import { type ClientViewCompanyEntity_companyEntity } from './__generated__/ClientViewCompanyEntity_companyEntity.graphql';



type Props = {|
  +companyEntity: ClientViewCompanyEntity_companyEntity,
|};
function ClientViewCompanyEntity(props: Props): React.Node {
  const { companyEntity } = props;

  return (
    <Expander
      headerFunc={ () => (
        <Group
          justifyContent='space-between'
        >
          <Group>
            <Group
              flexDirection='column'
            >
              <Group>
                <TextInput
                  value={ companyEntity.legalName }
                  onChange={ () => { } }
                />
              </Group>
              <SelectInput
                value={ companyEntity.companyType }
                onChange={ () => { } }
                options={ CompanyEntityTypeEnumPairs }
              />
            </Group>
          </Group>
          <Group>
            <Button
              label='Documents'
            />
            <Button
              label='Edit'
            />
          </Group>
        </Group>
      ) }
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
            <AddressDisplay address={ companyEntity.mailingAddress }/>
          </DisplayField>
          <DisplayField
            label='Physical Address'
          >
            <VisibilityControl
              visible={ companyEntityApplicableWhenForPhysicalAddress(companyEntity) }
            >
              <AddressDisplay address={ companyEntity.physicalAddress }/>
            </VisibilityControl>
          </DisplayField>
        </Group>
        <Group
          justifyContent='space-between'
        >
          <Group>
            <Checkbox
              checked={ companyEntity.isPrimary }
              label='Primary Entity'
              disabled={ companyEntity.isPrimary }
              onChange={ setCompanyEntityAsPrimary(companyEntity.id) }
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
        ...AddressDisplay_address
      }
      mailingAddressIsPhysicalAddress
      physicalAddress {
        id
        ...AddressDisplay_address
      }
    }
  `,
});
