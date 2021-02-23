// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import SelectInput from 'latitude/select/SelectInput';

import TextDisplay from 'react_lib/display/TextDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';

import { createDefaultAddress } from 'client_page/entities/Address';
import { companyEntityCalculateErrors, CompanyEntityTypeEnumPairs, type CompanyEntity } from 'client_page/entities/CompanyEntity';
import { createDefaultCountry } from 'client_page/entities/Country';
import { createDefaultCtpatReview } from 'client_page/entities/CtpatReview';
import { createDefaultCurrency } from 'client_page/entities/Currency';
import { createDefaultNetsuiteVendor } from 'client_page/entities/NetsuiteVendor';
import TabbedPane from '../../../src/react_lib/tab/TabbedPane';



type Props = {|
  +companyEntity: CompanyEntity,
  +onChange: (companyEntity: CompanyEntity) => void,
|};
function CompanyEntityForm(props: Props): React.Node {
  const { companyEntity, onChange } = props;

  return (
    <FormProvider
      value={ { errors: companyEntityCalculateErrors(companyEntity) } }
    >
      <FormField
        editorFor='legalName'
        label='Legal Name'
      >
        <TextInput
          value={ companyEntity.legalName }
          onChange={ (value) => {
            onChange({ ...companyEntity, legalName: value })
          } }
        />
      </FormField>
      <FormField
        editorFor='doingBusinessAs'
        label='Doing Business As'
      >
        <TextInput
          value={ companyEntity.doingBusinessAs }
          onChange={ (value) => {
            onChange({ ...companyEntity, doingBusinessAs: value })
          } }
        />
      </FormField>
      <FormField
        editorFor='companyType'
        label='Type of Business'
      >
        <SelectInput
          value={ companyEntity.companyType }
          onChange={ (value) => {
            onChange({ ...companyEntity, companyType: value })
          } }
          options={ CompanyEntityTypeEnumPairs }
        />
      </FormField>
      <FormField
        editorFor='countryOfBusinessRegistration'
        label='Country / Region of Business Registration'
      >
        <AssociationEditor
          id={ companyEntity.countryOfBusinessRegistration }
          onChange={ (value) => {
            onChange({ ...companyEntity, countryOfBusinessRegistration: value })
          } }
          isNullable={ true }
          query={ countriesQuery }
          toString={ x => x.toStringRepresentation }
        />
      </FormField>
      <FormField
        editorFor='stateOfBusinessRegistration'
        label='State / Region of Business Registration'
      >
        <TextInput
          value={ companyEntity.stateOfBusinessRegistration }
          onChange={ (value) => {
            onChange({ ...companyEntity, stateOfBusinessRegistration: value })
          } }
        />
      </FormField>
      <FormField
        editorFor='usTaxId'
        toolTip='Tax Id can be one of the following...'
        label='US Tax Id'
      >
        <TextInput
          value={ companyEntity.usTaxId }
          onChange={ (value) => {
            onChange({ ...companyEntity, usTaxId: value })
          } }
        />
      </FormField>
      <Group
        flexDirection='column'
      >
        <TextDisplay
          value='Mailing Address'
        />
        <Separator/>
        <FormField
          editorFor='mailingAddress.theAddress'
          label='Address'
        >
          <TextInput
            value={ companyEntity.mailingAddress.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(companyEntity));
              newObj.mailingAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <FormField
          editorFor='mailingAddress.theAddress2'
          label='Address 2'
        >
          <TextInput
            value={ companyEntity.mailingAddress.theAddress2 }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(companyEntity));
              newObj.mailingAddress.theAddress2 = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <Group>
          <FormField
            editorFor='mailingAddress.country'
            label='Country/Region'
          >
            <AssociationEditor
              id={ companyEntity.mailingAddress.country }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(companyEntity));
                newObj.mailingAddress.country = value;
                onChange(newObj);
              } }
              isNullable={ false }
              query={ countriesQuery }
              toString={ x => x.toStringRepresentation }
            />
          </FormField>
          <FormField
            editorFor='mailingAddress.stateOrProvince'
            label='State Or Province'
          >
            <AssociationEditor
              id={ companyEntity.mailingAddress.stateOrProvince }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(companyEntity));
                newObj.mailingAddress.stateOrProvince = value;
                onChange(newObj);
              } }
              isNullable={ false }
              query={ stateOrProvincesQuery }
              toString={ x => x.toStringRepresentation }
            />
          </FormField>
        </Group>
        <Group>
          <FormField
            editorFor='mailingAddress.city'
            label='City/Town'
            maxWidth={ 400 }
          >
            <TextInput
              value={ companyEntity.mailingAddress.city }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(companyEntity));
                newObj.mailingAddress.city = value;
                onChange(newObj);
              } }
            />
          </FormField>
          <FormField
            editorFor='mailingAddress.postalCode'
            label='Zip or Postal Code'
            maxWidth={ 150 }
          >
            <TextInput
              value={ companyEntity.mailingAddress.postalCode }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(companyEntity));
                newObj.mailingAddress.postalCode = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </Group>
      </Group>
      <TabbedPane
        tabs={[
          {
            id: "address",
            label: "Address",
            displayFunc: () => <TextDisplay value="Address Content"/>
          },
          {
            id: "tax_info",
            label: "Tax Information",
            displayFunc: () => <TextDisplay value="Tax Info Content"/>
          },
        ]}
      />
      <FormSubmitButton
        onClick={ () => save(companyEntity) }
      />
    </FormProvider>
  );
}

type StatefulProps = {|
  +companyEntity: CompanyEntity,
|};
export function CompanyEntityFormStateful(props: StatefulProps): React.Node {
  const companyEntity = relayToInternal(props.companyEntity);
  const [editedCompanyEntity, setEditedCompanyEntity] = React.useState(companyEntity);
  return <CompanyEntityForm
    companyEntity={ editedCompanyEntity }
    onChange={ setEditedCompanyEntity }
  />
}

function relayToInternal(relay: any): CompanyEntity {
  return {
    ...relay,
    physicalAddress: relay.physicalAddress || createDefaultAddress(),
    netsuiteVendorId: relay.netsuiteVendorId || createDefaultNetsuiteVendor(),
    ctpatReview: relay.ctpatReview || createDefaultCtpatReview(),
    countryOfBusinessRegistration: relay.countryOfBusinessRegistration || createDefaultCountry(),
    invoiceCurrencyDefault: relay.invoiceCurrencyDefault || createDefaultCurrency(),
  };
}

function save(companyEntity: CompanyEntity) {
  const variables = {
    id: companyEntity.id,
    legalName: companyEntity.legalName,
    doingBusinessAs: companyEntity.doingBusinessAs,
    companyType: companyEntity.companyType,
    stateOfBusinessRegistration: companyEntity.stateOfBusinessRegistration,
    usTaxId: companyEntity.usTaxId,
    isPrimary: companyEntity.isPrimary,
    dgDisclaimerAgreed: companyEntity.dgDisclaimerAgreed,
    mailingAddressIsPhysicalAddress: companyEntity.mailingAddressIsPhysicalAddress,
    brBlCompanyName: companyEntity.brBlCompanyName,
    isArchived: companyEntity.isArchived,
    brBlRegistrationNumber: companyEntity.brBlRegistrationNumber,
    brBlAddress: companyEntity.brBlAddress,
    brBlLegalRepChinese: companyEntity.brBlLegalRepChinese,
    brBlLegalRepPinyin: companyEntity.brBlLegalRepPinyin,
    usFccNumber: companyEntity.usFccNumber,
    eoriNumber: companyEntity.eoriNumber,
    usciNumber: companyEntity.usciNumber,
    agentIataCode: companyEntity.agentIataCode,
    hkRaNumber: companyEntity.hkRaNumber,
    vendorCategory: companyEntity.vendorCategory,
    mailingAddress: companyEntity.mailingAddress,
    physicalAddress: companyEntity.physicalAddress,
    vatNumbers: companyEntity.vatNumbers,
    netsuiteVendorId: companyEntity.netsuiteVendorId,
    ctpatReview: companyEntity.ctpatReview,
    documents: companyEntity.documents,
    countryOfBusinessRegistration: companyEntity.countryOfBusinessRegistration,
    invoiceCurrencyDefault: companyEntity.invoiceCurrencyDefault,
  };

  basicCommitMutation(mutation, variables);
}

const mutation = graphql`
  mutation CompanyEntityFormMutation(
    $id: String!
    $legalName: String!
    $doingBusinessAs: String!
    $companyType: CompanyEntityTypeEnum!
    $stateOfBusinessRegistration: String!
    $usTaxId: String!
    $isPrimary: Boolean!
    $dgDisclaimerAgreed: Boolean!
    $mailingAddressIsPhysicalAddress: Boolean!
    $brBlCompanyName: String!
    $isArchived: Boolean!
    $brBlRegistrationNumber: String!
    $brBlAddress: String!
    $brBlLegalRepChinese: String!
    $brBlLegalRepPinyin: String!
    $usFccNumber: String!
    $eoriNumber: String!
    $usciNumber: String!
    $agentIataCode: String!
    $hkRaNumber: String!
    $vendorCategory: VendorCategoryEnum
    $mailingAddress: AddressInput!
    $physicalAddress: AddressInput
    $vatNumbers: [VatNumberInput!]!
    $netsuiteVendorId: String
    $ctpatReview: CtpatReviewInput
    $documents: [DocumentInput!]!
    $countryOfBusinessRegistration: String
    $invoiceCurrencyDefault: String
  ) {
    createOrUpdateCompanyEntity(
      id: $id
      legalName: $legalName
      doingBusinessAs: $doingBusinessAs
      companyType: $companyType
      stateOfBusinessRegistration: $stateOfBusinessRegistration
      usTaxId: $usTaxId
      isPrimary: $isPrimary
      dgDisclaimerAgreed: $dgDisclaimerAgreed
      mailingAddressIsPhysicalAddress: $mailingAddressIsPhysicalAddress
      brBlCompanyName: $brBlCompanyName
      isArchived: $isArchived
      brBlRegistrationNumber: $brBlRegistrationNumber
      brBlAddress: $brBlAddress
      brBlLegalRepChinese: $brBlLegalRepChinese
      brBlLegalRepPinyin: $brBlLegalRepPinyin
      usFccNumber: $usFccNumber
      eoriNumber: $eoriNumber
      usciNumber: $usciNumber
      agentIataCode: $agentIataCode
      hkRaNumber: $hkRaNumber
      vendorCategory: $vendorCategory
      mailingAddress: $mailingAddress
      physicalAddress: $physicalAddress
      vatNumbers: $vatNumbers
      netsuiteVendorIdId: $netsuiteVendorId
      ctpatReview: $ctpatReview
      documents: $documents
      countryOfBusinessRegistrationId: $countryOfBusinessRegistration
      invoiceCurrencyDefaultId: $invoiceCurrencyDefault
    )
  }
`;

// $FlowExpectedError
export default createFragmentContainer(CompanyEntityFormStateful, {
  companyEntity: graphql`
    fragment CompanyEntityForm_companyEntity on CompanyEntity {
      id
      companyType
      countryOfBusinessRegistration {
        id
      }
      doingBusinessAs
      legalName
      mailingAddress {
        id
        city
        country {
          id
        }
        postalCode
        stateOrProvince {
          id
        }
        theAddress
        theAddress2
      }
      stateOfBusinessRegistration
      usTaxId
    }
  `,
});

const countriesQuery = graphql`
  query CompanyEntityForm_countriesQuery {
    entities: countries {
      id
      toStringRepresentation
    }
  }
`;

const stateOrProvincesQuery = graphql`
  query CompanyEntityForm_stateOrProvincesQuery {
    entities: stateOrProvinces {
      id
      toStringRepresentation
    }
  }
`;

