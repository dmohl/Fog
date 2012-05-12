<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="FogAzure" generation="1" functional="0" release="0" Id="e640fc8e-c75b-4b65-8954-981c0df7e149" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="FogAzureGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="Fog.Intergration.Tests:BlobStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:BlobStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:QueueStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:QueueStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:ServiceBusIssuer" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:ServiceBusIssuer" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:ServiceBusKey" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:ServiceBusKey" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:ServiceBusNamespace" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:ServiceBusNamespace" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:ServiceBusScheme" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:ServiceBusScheme" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:ServiceBusServicePath" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:ServiceBusServicePath" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TableStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TableStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestBlobStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestBlobStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestQueueStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestQueueStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestServiceBusIssuer" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestServiceBusIssuer" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestServiceBusKey" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestServiceBusKey" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestServiceBusNamespace" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestServiceBusNamespace" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestServiceBusScheme" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestServiceBusScheme" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestServiceBusServicePath" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestServiceBusServicePath" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.Tests:TestTableStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.Tests:TestTableStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="Fog.Intergration.TestsInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/FogAzure/FogAzureGroup/MapFog.Intergration.TestsInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapFog.Intergration.Tests:BlobStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/BlobStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:QueueStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/QueueStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:ServiceBusIssuer" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/ServiceBusIssuer" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:ServiceBusKey" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/ServiceBusKey" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:ServiceBusNamespace" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/ServiceBusNamespace" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:ServiceBusScheme" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/ServiceBusScheme" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:ServiceBusServicePath" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/ServiceBusServicePath" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TableStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TableStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestBlobStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestBlobStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestQueueStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestQueueStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestServiceBusIssuer" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestServiceBusIssuer" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestServiceBusKey" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestServiceBusKey" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestServiceBusNamespace" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestServiceBusNamespace" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestServiceBusScheme" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestServiceBusScheme" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestServiceBusServicePath" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestServiceBusServicePath" />
          </setting>
        </map>
        <map name="MapFog.Intergration.Tests:TestTableStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.Tests/TestTableStorageConnectionString" />
          </setting>
        </map>
        <map name="MapFog.Intergration.TestsInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.TestsInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Fog.Intergration.Tests" generation="1" functional="0" release="0" software="C:\git\Fog\FogAzure\csx\Debug\roles\Fog.Intergration.Tests" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="BlobStorageConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="QueueStorageConnectionString" defaultValue="" />
              <aCS name="ServiceBusIssuer" defaultValue="" />
              <aCS name="ServiceBusKey" defaultValue="" />
              <aCS name="ServiceBusNamespace" defaultValue="" />
              <aCS name="ServiceBusScheme" defaultValue="" />
              <aCS name="ServiceBusServicePath" defaultValue="" />
              <aCS name="TableStorageConnectionString" defaultValue="" />
              <aCS name="TestBlobStorageConnectionString" defaultValue="" />
              <aCS name="TestQueueStorageConnectionString" defaultValue="" />
              <aCS name="TestServiceBusIssuer" defaultValue="" />
              <aCS name="TestServiceBusKey" defaultValue="" />
              <aCS name="TestServiceBusNamespace" defaultValue="" />
              <aCS name="TestServiceBusScheme" defaultValue="" />
              <aCS name="TestServiceBusServicePath" defaultValue="" />
              <aCS name="TestTableStorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Fog.Intergration.Tests&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Fog.Intergration.Tests&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.TestsInstances" />
            <sCSPolicyFaultDomainMoniker name="/FogAzure/FogAzureGroup/Fog.Intergration.TestsFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Fog.Intergration.TestsFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Fog.Intergration.TestsInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>