<xsl:stylesheet version="1.0"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
 xmlns:user="urn:my-scripts"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" >


  <xsl:template match="/">
    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
      xmlns:o="urn:schemas-microsoft-com:office:office"
      xmlns:x="urn:schemas-microsoft-com:office:excel"
      xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
      xmlns:html="http://www.w3.org/TR/REC-html40">
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Center"/>
          <Borders/>
          <Font ss:FontName="宋体" x:CharSet="134" ss:Size="12"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="shead" ss:Name="Normal 3">
          <Alignment ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
          </Borders>
          <Font x:Family="Swiss"/>
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="sbody" ss:Name="Normal 4">
          <Alignment ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#333333"/>
          </Borders>
          <Font x:Family="Swiss"/>
          <Interior />
        </Style>
      </Styles>
      <xsl:apply-templates/>
    </Workbook>
  </xsl:template>


  <xsl:template match="/*">
    <Worksheet>
      <xsl:attribute name="ss:Name">
        <xsl:value-of select="local-name(/*/*)" />
      </xsl:attribute>
      <Table x:FullColumns="1" x:FullRows="1">
        <Row>
          <xsl:for-each select="/*/fields/*">
            <Cell ss:StyleID="shead">
              <Data ss:Type="String">
                <xsl:value-of select="local-name()" />
              </Data>
            </Cell>
          </xsl:for-each>
        </Row>
        <xsl:apply-templates select="/*/record"/>
      </Table>
    </Worksheet>
  </xsl:template>


  <xsl:template match="/*/record">
    <Row>
      <xsl:apply-templates/>
    </Row>
  </xsl:template>


  <xsl:template match="/*/*/*">
    <Cell ss:StyleID="sbody">
      <Data ss:Type="String">
        <xsl:value-of select="." />
      </Data>
    </Cell>
  </xsl:template>


</xsl:stylesheet>
