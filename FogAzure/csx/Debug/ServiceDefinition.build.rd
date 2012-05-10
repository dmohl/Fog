<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="FogAzure" generation="1" functional="0" release="0" Id="16604afd-774d-401f-85d5-d0718df63300" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
          <role name="Fog.Intergration.Tests" generation="1" functional="0" release="0" software="C:\git\Fog\FogAzure\csx\Debug\roles\Fog.Intergration.Tests" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="BlobStorageConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="TableStorageConnectionString" defaultValue="" />
              <aCS name="TestBlobStorageConnectionString" defaultValue="" />
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