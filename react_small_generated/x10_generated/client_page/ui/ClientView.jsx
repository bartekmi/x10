// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import SelectInput from 'latitude/select/SelectInput';
import Text from 'latitude/Text';

import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';

import { ClientPrimaryShipmentRoleEnumPairs, ClientPurchasingBehaviorEnumPairs, ClientSegmentEnumPairs, ClientStatusEnumPairs, type Client } from 'client_page/entities/Client';
import { createDefaultCompanyEntity } from 'client_page/entities/CompanyEntity';
import { contactName } from 'client_page/entities/Contact';
import ClientViewCompanyEntity from 'client_page/ui/ClientViewCompanyEntity';

import { type ClientView_client } from './__generated__/ClientView_client.graphql';



type Props = {|
  +client: ClientView_client,
|};
function ClientView(props: Props): React.Node {
  const { client } = props;

  return (
    <DisplayForm>
      <Group
        justifyContent='space-between'
      >
        <Text
          weight='bold'
          children={ client.company?.primaryEntity?.legalName }
        />
        <Button
          label='Edit'
        />
      </Group>
      <Separator/>
      <Group
        justifyContent='space-between'
      >
        <DisplayField
          label='Primary Contact'
        >
          <Group
            flexDirection='column'
          >
            <TextInput
              value={ contactName(client.primaryContact) }
              onChange={ () => { } }
              readOnly={ true }
            />
            <TextInput
              value={ client.primaryContact?.email }
              onChange={ () => { } }
              readOnly={ true }
            />
            <TextInput
              value={ client.primaryContact?.phone }
              onChange={ () => { } }
              readOnly={ true }
            />
          </Group>
        </DisplayField>
        <DisplayField
          label='Saleforce Account Ref:'
        >
          <Group
            flexDirection='column'
          >
            <TextInput
              value={ client.salesforceAccountRef }
              onChange={ () => { } }
              readOnly={ true }
            />
            <Button
              label='Force Outbound Salesforce Sync'
            />
          </Group>
        </DisplayField>
        <DisplayField
          label='Referred By'
        >
          <TextInput
            value={ client.referredBy }
            onChange={ () => { } }
            readOnly={ true }
          />
        </DisplayField>
      </Group>
      <Group
        justifyContent='space-between'
      >
        <DisplayField
          label='Status'
        >
          <SelectInput
            value={ client.status }
            onChange={ () => { } }
            disabled={ true }
            options={ ClientStatusEnumPairs }
          />
        </DisplayField>
        <DisplayField
          label='Segment'
        >
          <SelectInput
            value={ client.segment }
            onChange={ () => { } }
            disabled={ true }
            options={ ClientSegmentEnumPairs }
          />
        </DisplayField>
        <DisplayField
          label='Purchasing Behavior'
        >
          <SelectInput
            value={ client.purchasingBehavior }
            onChange={ () => { } }
            disabled={ true }
            options={ ClientPurchasingBehaviorEnumPairs }
          />
        </DisplayField>
        <DisplayField
          label='Website'
        >
          <TextInput
            value={ client.company?.website }
            onChange={ () => { } }
            readOnly={ true }
          />
        </DisplayField>
        <Group
          flexDirection='column'
        >
          <Button
            label='Solicit RFQ'
          />
          <Button
            label='Edit NSA Lanes'
          />
          <Button
            label='App Features'
          />
        </Group>
      </Group>
      <Group
        gap={ 100 }
      >
        <DisplayField
          label='Primary Shipment Role'
        >
          <SelectInput
            value={ client.primaryShipmentRole }
            onChange={ () => { } }
            disabled={ true }
            options={ ClientPrimaryShipmentRoleEnumPairs }
          />
        </DisplayField>
        <DisplayField
          toolTip='Count of invoiced shipments...'
          label='Shipment History'
        >
          <Group
            gap={ 20 }
          >
            <Group>
              <Text
                weight='bold'
                children={ client.shipmentsAsClient }
              />
              <Text
                children='as Client'
              />
            </Group>
            <Group>
              <Text
                weight='bold'
                children={ client.shipmentsAsShipper }
              />
              <Text
                children='as Shipper'
              />
            </Group>
            <Group>
              <Text
                weight='bold'
                children={ client.shipmentsAsConsignee }
              />
              <Text
                children='as Consignee'
              />
            </Group>
          </Group>
        </DisplayField>
      </Group>
      <MultiStacker
        items={ client.company?.entities }
        onChange={ () => { } }
        itemDisplayFunc={ (data, onChange) => (
          <ClientViewCompanyEntity companyEntity={ data }/>
        ) }
        addNewItem={ createDefaultCompanyEntity }
      />
    </DisplayForm>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ClientView, {
  client: graphql`
    fragment ClientView_client on Client {
      id
      company {
        id
        entities {
          id
          ...ClientViewCompanyEntity_companyEntity
        }
        primaryEntity {
          id
          legalName
        }
        website
      }
      primaryContact {
        id
        email
        firstName
        lastName
        phone
      }
      primaryShipmentRole
      purchasingBehavior
      referredBy
      salesforceAccountRef
      segment
      shipmentsAsClient
      shipmentsAsConsignee
      shipmentsAsShipper
      status
    }
  `,
});
