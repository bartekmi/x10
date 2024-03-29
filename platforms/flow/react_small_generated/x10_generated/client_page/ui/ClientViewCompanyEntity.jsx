// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Checkbox from 'latitude/Checkbox';
import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';
import Status from 'latitude/Status';

import BooleanBanner from 'react_lib/display/BooleanBanner';
import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Dialog from 'react_lib/modal/Dialog';
import StyleControl from 'react_lib/StyleControl';

import ctpatReviewStatusToIntent from 'client_page/ctpatReviewStatusToIntent';
import ctpatReviewStatusToText from 'client_page/ctpatReviewStatusToText';
import { companyEntityApplicableWhenForPhysicalAddress, CompanyEntityTypeEnumPairs, type CompanyEntity } from 'client_page/entities/CompanyEntity';
import { CtpatReviewStatusEnumPairs } from 'client_page/entities/CtpatReview';
import setCompanyEntityAsPrimary from 'client_page/setCompanyEntityAsPrimary';
import AddressDisplay from 'client_page/ui/AddressDisplay';
import ClientViewDocuments from 'client_page/ui/ClientViewDocuments';
import CompanyEntityForm from 'client_page/ui/CompanyEntityForm';

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
          <Group
            alignItems='center'
          >
            <Group
              alignItems='center'
              gap={ 20 }
            >
              <VerticalStackPanel
                gap={ 0 }
              >
                <TextDisplay
                  value={ companyEntity?.legalName }
                />
                <EnumDisplay
                  value={ companyEntity?.companyType }
                  options={ CompanyEntityTypeEnumPairs }
                />
              </VerticalStackPanel>
              <BooleanBanner
                value={ companyEntity?.isPrimary }
                label='Primary Entity'
                icon='star'
              />
              <StyleControl
                visible={ companyEntity?.ctpatReview != null }
              >
                <DisplayField
                  label='C-TPAT STATUS'
                >
                  <Status
                    intent={ ctpatReviewStatusToIntent(companyEntity?.ctpatReview?.status) }
                    children={ ctpatReviewStatusToText(companyEntity?.ctpatReview?.status) }
                  />
                </DisplayField>
              </StyleControl>
            </Group>
          </Group>
          <Group
            alignItems='center'
          >
            <Dialog
              title='Company Entity Documents'
              openButton={
                <Button
                  label='Documents'
                />
              }
            >
              <ClientViewDocuments companyEntity={ companyEntity }/>
            </Dialog>
            <Dialog
              title='Edit Company Entity'
              openButton={
                <Button
                  label='Edit'
                />
              }
            >
              <CompanyEntityForm companyEntity={ companyEntity }/>
            </Dialog>
          </Group>
        </Group>
      ) }
    >
      <VerticalStackPanel
        gap={ 20 }
      >
        <Group
          alignItems='center'
          gap={ 80 }
        >
          <DisplayField
            label='Core Id'
          >
            <FloatDisplay
              value={ companyEntity?.coreId }
            />
          </DisplayField>
          <DisplayField
            label='Mailing Address'
          >
            <AddressDisplay address={ companyEntity.mailingAddress }/>
          </DisplayField>
          <DisplayField
            label='Physical Address'
          >
            <StyleControl
              visible={ companyEntityApplicableWhenForPhysicalAddress(companyEntity) }
            >
              <AddressDisplay address={ companyEntity.physicalAddress }/>
            </StyleControl>
          </DisplayField>
        </Group>
        <Group
          justifyContent='space-between'
        >
          <Group
            alignItems='center'
          >
            <Checkbox
              checked={ companyEntity?.isPrimary?.isPrimary }
              label='Primary Entity'
              disabled={ companyEntity?.isPrimary?.isPrimary }
              onChange={ setCompanyEntityAsPrimary(companyEntity?.isPrimary?.id) }
            />
            <StyleControl
              visible={ companyEntity?.isPrimary }
            >
              <HelpTooltip
                text='You can change the primary entity by selecting...'
              />
            </StyleControl>
          </Group>
          <Button
            label='Archive this entity'
          />
        </Group>
        <DisplayForm>
          <StyleControl
            visible={ companyEntity?.ctpatReview == null }
          >
            <Group
              justifyContent='space-between'
            >
              <DisplayField
                label='Admin Email'
              >
                <TextDisplay
                  value={ companyEntity?.adminEmail }
                />
              </DisplayField>
              <Button
                label='Initiate First-Time Compliance Screen'
              />
            </Group>
          </StyleControl>
          <StyleControl
            visible={ companyEntity?.ctpatReview != null }
          >
            <Group
              justifyContent='space-between'
            >
              <Group
                alignItems='center'
              >
                <Checkbox
                  checked={ companyEntity?.ctpatReview?.complianceScreenRequired }
                  label='Compliance Screen Required'
                  disabled={ true }
                />
                <HelpTooltip
                  text='C-TPAT Screening is required...'
                />
              </Group>
              <DisplayField
                label='Status'
              >
                <EnumDisplay
                  value={ companyEntity?.ctpatReview?.status }
                  options={ CtpatReviewStatusEnumPairs }
                />
              </DisplayField>
              <DisplayField
                label='Next due date'
              >
                <DateDisplay
                  value={ companyEntity?.ctpatReview?.expiresAt }
                />
              </DisplayField>
              <DisplayField
                label='Compliance Contact Email'
              >
                <TextDisplay
                  value={ companyEntity?.ctpatReview?.complianceContactEmail }
                />
              </DisplayField>
              <Group
                alignItems='center'
              >
                <Button
                  label='Request Screen'
                />
                <Button
                  label='Edit'
                />
              </Group>
            </Group>
          </StyleControl>
        </DisplayForm>
      </VerticalStackPanel>
    </Expander>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ClientViewCompanyEntity, {
  companyEntity: graphql`
    fragment ClientViewCompanyEntity_companyEntity on CompanyEntity {
      id
      adminEmail
      companyType
      coreId
      ctpatReview {
        id
        complianceContactEmail
        complianceScreenRequired
        expiresAt
        status
      }
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
      ...ClientViewDocuments_companyEntity
      ...CompanyEntityForm_companyEntity
    }
  `,
});

