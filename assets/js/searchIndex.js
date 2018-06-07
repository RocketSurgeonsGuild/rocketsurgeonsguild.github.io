
var camelCaseTokenizer = function (obj) {
    var previous = '';
    return obj.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
        var current = cur.toLowerCase();
        if(acc.length === 0) {
            previous = current;
            return acc.concat(current);
        }
        previous = previous.concat(current);
        return acc.concat([current, previous]);
    }, []);
}
lunr.tokenizer.registerFunction(camelCaseTokenizer, 'camelCaseTokenizer')
var searchModule = function() {
    var idMap = [];
    function y(e) { 
        idMap.push(e); 
    }
    var idx = lunr(function() {
        this.field('title', { boost: 10 });
        this.field('content');
        this.field('description', { boost: 5 });
        this.field('tags', { boost: 50 });
        this.ref('id');
        this.tokenizer(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
    });
    function a(e) { 
        idx.add(e); 
    }

    a({
        id:0,
        title:"UnionAttribute",
        content:"UnionAttribute",
        description:'',
        tags:''
    });

    a({
        id:1,
        title:"AsyncLinqExtensions",
        content:"AsyncLinqExtensions",
        description:'',
        tags:''
    });

    a({
        id:2,
        title:"EnsureContainerIsRunningExtensions",
        content:"EnsureContainerIsRunningExtensions",
        description:'',
        tags:''
    });

    a({
        id:3,
        title:"Builder",
        content:"Builder",
        description:'',
        tags:''
    });

    a({
        id:4,
        title:"DependencyContextAssemblyCandidateFinder",
        content:"DependencyContextAssemblyCandidateFinder",
        description:'',
        tags:''
    });

    a({
        id:5,
        title:"TFBuildCakeAliases",
        content:"TFBuildCakeAliases",
        description:'',
        tags:''
    });

    a({
        id:6,
        title:"Settings XUnitSettings",
        content:"Settings XUnitSettings",
        description:'',
        tags:''
    });

    a({
        id:7,
        title:"EncodingType",
        content:"EncodingType",
        description:'',
        tags:''
    });

    a({
        id:8,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:9,
        title:"NodaDateTimeZoneConverter",
        content:"NodaDateTimeZoneConverter",
        description:'',
        tags:''
    });

    a({
        id:10,
        title:"XunitLoggerProvider",
        content:"XunitLoggerProvider",
        description:'',
        tags:''
    });

    a({
        id:11,
        title:"MethodNotFoundException",
        content:"MethodNotFoundException",
        description:'',
        tags:''
    });

    a({
        id:12,
        title:"IJsonBinder",
        content:"IJsonBinder",
        description:'',
        tags:''
    });

    a({
        id:13,
        title:"DotCoverReportsSettings",
        content:"DotCoverReportsSettings",
        description:'',
        tags:''
    });

    a({
        id:14,
        title:"CommonCakeAliases",
        content:"CommonCakeAliases",
        description:'',
        tags:''
    });

    a({
        id:15,
        title:"CodeCoverageAliases",
        content:"CodeCoverageAliases",
        description:'',
        tags:''
    });

    a({
        id:16,
        title:"TopographicalSortExtensions",
        content:"TopographicalSortExtensions",
        description:'',
        tags:''
    });

    a({
        id:17,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:18,
        title:"Builder",
        content:"Builder",
        description:'',
        tags:''
    });

    a({
        id:19,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:20,
        title:"SimpleConventionScanner",
        content:"SimpleConventionScanner",
        description:'',
        tags:''
    });

    a({
        id:21,
        title:"UnionHelper",
        content:"UnionHelper",
        description:'',
        tags:''
    });

    a({
        id:22,
        title:"ConventionContainerBuilder",
        content:"ConventionContainerBuilder",
        description:'',
        tags:''
    });

    a({
        id:23,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:24,
        title:"ObservableExtensions",
        content:"ObservableExtensions",
        description:'',
        tags:''
    });

    a({
        id:25,
        title:"UnionKeyAttribute",
        content:"UnionKeyAttribute",
        description:'',
        tags:''
    });

    a({
        id:26,
        title:"InjectableMethodBuilderBase",
        content:"InjectableMethodBuilderBase",
        description:'',
        tags:''
    });

    a({
        id:27,
        title:"TestResultsType",
        content:"TestResultsType",
        description:'',
        tags:''
    });

    a({
        id:28,
        title:"ConventionComposer",
        content:"ConventionComposer",
        description:'',
        tags:''
    });

    a({
        id:29,
        title:"BackingFieldHelper",
        content:"BackingFieldHelper",
        description:'',
        tags:''
    });

    a({
        id:30,
        title:"Base Url",
        content:"Base Url",
        description:'',
        tags:''
    });

    a({
        id:31,
        title:"ReportCodeCoverageOptions",
        content:"ReportCodeCoverageOptions",
        description:'',
        tags:''
    });

    a({
        id:32,
        title:"JsonBinderExtensions",
        content:"JsonBinderExtensions",
        description:'',
        tags:''
    });

    a({
        id:33,
        title:"HostingEnvironmentExtensions",
        content:"HostingEnvironmentExtensions",
        description:'',
        tags:''
    });

    a({
        id:34,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:35,
        title:"ApplicationServicesBuilder",
        content:"ApplicationServicesBuilder",
        description:'',
        tags:''
    });

    a({
        id:36,
        title:"PrivateSetterContractResolver",
        content:"PrivateSetterContractResolver",
        description:'',
        tags:''
    });

    a({
        id:37,
        title:"ValueExtensions",
        content:"ValueExtensions",
        description:'',
        tags:''
    });

    a({
        id:38,
        title:"ChildServicesBuilder",
        content:"ChildServicesBuilder",
        description:'',
        tags:''
    });

    a({
        id:39,
        title:"AppDomainAssemblyProvider",
        content:"AppDomainAssemblyProvider",
        description:'',
        tags:''
    });

    a({
        id:40,
        title:"GitVersion",
        content:"GitVersion",
        description:'',
        tags:''
    });

    a({
        id:41,
        title:"CustomNodaConverters",
        content:"CustomNodaConverters",
        description:'',
        tags:''
    });

    a({
        id:42,
        title:"AppDomainAssemblyCandidateFinder",
        content:"AppDomainAssemblyCandidateFinder",
        description:'',
        tags:''
    });

    a({
        id:43,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:44,
        title:"EnvironmentName",
        content:"EnvironmentName",
        description:'',
        tags:''
    });

    a({
        id:45,
        title:"UploadArtifactsOptions",
        content:"UploadArtifactsOptions",
        description:'',
        tags:''
    });

    a({
        id:46,
        title:"IEnsureContainerIsRunningContext",
        content:"IEnsureContainerIsRunningContext",
        description:'',
        tags:''
    });

    a({
        id:47,
        title:"ServicesBuilder",
        content:"ServicesBuilder",
        description:'',
        tags:''
    });

    a({
        id:48,
        title:"AzureStorageEmulatorAutomation",
        content:"AzureStorageEmulatorAutomation",
        description:'',
        tags:''
    });

    a({
        id:49,
        title:"LoggingBuilder",
        content:"LoggingBuilder",
        description:'',
        tags:''
    });

    a({
        id:50,
        title:"InformationProvider",
        content:"InformationProvider",
        description:'',
        tags:''
    });

    a({
        id:51,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:52,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:53,
        title:"HostingEnvironment",
        content:"HostingEnvironment",
        description:'',
        tags:''
    });

    a({
        id:54,
        title:"Base Url CharMap",
        content:"Base Url CharMap",
        description:'',
        tags:''
    });

    a({
        id:55,
        title:"Settings PackSettings",
        content:"Settings PackSettings",
        description:'',
        tags:''
    });

    a({
        id:56,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:57,
        title:"ConventionScannerExtensions",
        content:"ConventionScannerExtensions",
        description:'',
        tags:''
    });

    a({
        id:58,
        title:"ConventionComposerExtensions",
        content:"ConventionComposerExtensions",
        description:'',
        tags:''
    });

    a({
        id:59,
        title:"UnionConverter",
        content:"UnionConverter",
        description:'',
        tags:''
    });

    a({
        id:60,
        title:"ReportTestResultsOptions",
        content:"ReportTestResultsOptions",
        description:'',
        tags:''
    });

    a({
        id:61,
        title:"InstantPatternConverter",
        content:"InstantPatternConverter",
        description:'',
        tags:''
    });

    a({
        id:62,
        title:"JsonBinder",
        content:"JsonBinder",
        description:'',
        tags:''
    });

    a({
        id:63,
        title:"BackingFieldValueProvider",
        content:"BackingFieldValueProvider",
        description:'',
        tags:''
    });

    a({
        id:64,
        title:"Extensions",
        content:"Extensions",
        description:'',
        tags:''
    });

    a({
        id:65,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:66,
        title:"DefaultAssemblyCandidateFinder",
        content:"DefaultAssemblyCandidateFinder",
        description:'',
        tags:''
    });

    a({
        id:67,
        title:"OffsetDateTimePatternConverter",
        content:"OffsetDateTimePatternConverter",
        description:'',
        tags:''
    });

    a({
        id:68,
        title:"Base Url",
        content:"Base Url",
        description:'',
        tags:''
    });

    a({
        id:69,
        title:"ConventionContext",
        content:"ConventionContext",
        description:'',
        tags:''
    });

    a({
        id:70,
        title:"NodaMultiplePatternConverter",
        content:"NodaMultiplePatternConverter",
        description:'',
        tags:''
    });

    a({
        id:71,
        title:"TestBase",
        content:"TestBase",
        description:'',
        tags:''
    });

    a({
        id:72,
        title:"ConventionComposer",
        content:"ConventionComposer",
        description:'',
        tags:''
    });

    a({
        id:73,
        title:"CloudTableProvider",
        content:"CloudTableProvider",
        description:'',
        tags:''
    });

    a({
        id:74,
        title:"CodeCoverageType",
        content:"CodeCoverageType",
        description:'',
        tags:''
    });

    a({
        id:75,
        title:"ConventionBuilder",
        content:"ConventionBuilder",
        description:'',
        tags:''
    });

    a({
        id:76,
        title:"ComplexTableEntity",
        content:"ComplexTableEntity",
        description:'',
        tags:''
    });

    a({
        id:77,
        title:"DeconstructorExtensions",
        content:"DeconstructorExtensions",
        description:'',
        tags:''
    });

    a({
        id:78,
        title:"AutoTestBase",
        content:"AutoTestBase",
        description:'',
        tags:''
    });

    a({
        id:79,
        title:"TypeInfoExtensions",
        content:"TypeInfoExtensions",
        description:'',
        tags:''
    });

    a({
        id:80,
        title:"DefaultAssemblyProvider",
        content:"DefaultAssemblyProvider",
        description:'',
        tags:''
    });

    a({
        id:81,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:82,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:83,
        title:"Tfs",
        content:"Tfs",
        description:'',
        tags:''
    });

    a({
        id:84,
        title:"AzureStorageExtensions",
        content:"AzureStorageExtensions",
        description:'',
        tags:''
    });

    a({
        id:85,
        title:"Settings CoverageSettings",
        content:"Settings CoverageSettings",
        description:'',
        tags:''
    });

    a({
        id:86,
        title:"TfsCakeAliases",
        content:"TfsCakeAliases",
        description:'',
        tags:''
    });

    a({
        id:87,
        title:"TFBuildPullRequestInfo",
        content:"TFBuildPullRequestInfo",
        description:'',
        tags:''
    });

    a({
        id:88,
        title:"AzureStorageSettings",
        content:"AzureStorageSettings",
        description:'',
        tags:''
    });

    a({
        id:89,
        title:"PropertyGetter",
        content:"PropertyGetter",
        description:'',
        tags:''
    });

    a({
        id:90,
        title:"Settings",
        content:"Settings",
        description:'',
        tags:''
    });

    a({
        id:91,
        title:"InjectableMethodBuilder",
        content:"InjectableMethodBuilder",
        description:'',
        tags:''
    });

    a({
        id:92,
        title:"Base Encoding",
        content:"Base Encoding",
        description:'',
        tags:''
    });

    a({
        id:93,
        title:"ArtifactType",
        content:"ArtifactType",
        description:'',
        tags:''
    });

    a({
        id:94,
        title:"AggregateConventionScanner",
        content:"AggregateConventionScanner",
        description:'',
        tags:''
    });

    a({
        id:95,
        title:"LocalCakeAliases",
        content:"LocalCakeAliases",
        description:'',
        tags:''
    });

    a({
        id:96,
        title:"Composer",
        content:"Composer",
        description:'',
        tags:''
    });

    a({
        id:97,
        title:"ConventionComposerBase",
        content:"ConventionComposerBase",
        description:'',
        tags:''
    });

    a({
        id:98,
        title:"DependencyContextAssemblyProvider",
        content:"DependencyContextAssemblyProvider",
        description:'',
        tags:''
    });

    a({
        id:99,
        title:"BasicConventionScanner",
        content:"BasicConventionScanner",
        description:'',
        tags:''
    });

    a({
        id:100,
        title:"XunitLogger",
        content:"XunitLogger",
        description:'',
        tags:''
    });

    a({
        id:101,
        title:"ConventionScannerBase",
        content:"ConventionScannerBase",
        description:'',
        tags:''
    });

    a({
        id:102,
        title:"PropertyDelegate",
        content:"PropertyDelegate",
        description:'',
        tags:''
    });

    a({
        id:103,
        title:"LinqExtensions",
        content:"LinqExtensions",
        description:'',
        tags:''
    });

    y({
        url:'/api/Rocket.Surgery.Unions/UnionAttribute',
        title:"UnionAttribute",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Linq/AsyncLinqExtensions',
        title:"AsyncLinqExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing.Docker/EnsureContainerIsRunningExtensions',
        title:"EnsureContainerIsRunningExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Builders/Builder',
        title:"Builder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/DependencyContextAssemblyCandidateFinder',
        title:"DependencyContextAssemblyCandidateFinder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/TFBuildCakeAliases',
        title:"TFBuildCakeAliases",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/XUnitSettings',
        title:"Settings.XUnitSettings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Encoding/EncodingType',
        title:"EncodingType",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_11',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/NodaDateTimeZoneConverter',
        title:"NodaDateTimeZoneConverter",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing/XunitLoggerProvider',
        title:"XunitLoggerProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/MethodNotFoundException',
        title:"MethodNotFoundException",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Binding/IJsonBinder',
        title:"IJsonBinder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/DotCoverReportsSettings',
        title:"DotCoverReportsSettings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/CommonCakeAliases',
        title:"CommonCakeAliases",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/CodeCoverageAliases',
        title:"CodeCoverageAliases",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions/TopographicalSortExtensions',
        title:"TopographicalSortExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_1',
        title:"InjectableMethodBuilder<T>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Builders/Builder_1',
        title:"Builder<TBuilder>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_6',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Scanners/SimpleConventionScanner',
        title:"SimpleConventionScanner",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Unions/UnionHelper',
        title:"UnionHelper",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionContainerBuilder_3',
        title:"ConventionContainerBuilder<TBuilder, TConvention, TDelegate>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_8',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7, T8>",
        description:""
    });

    y({
        url:'/api/System.Reactive.Linq/ObservableExtensions',
        title:"ObservableExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Unions/UnionKeyAttribute',
        title:"UnionKeyAttribute",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilderBase',
        title:"InjectableMethodBuilderBase",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/TestResultsType',
        title:"TestResultsType",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionComposer',
        title:"ConventionComposer",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/BackingFieldHelper',
        title:"BackingFieldHelper",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Encoding/Base32Url',
        title:"Base32Url",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/ReportCodeCoverageOptions',
        title:"ReportCodeCoverageOptions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Binding/JsonBinderExtensions',
        title:"JsonBinderExtensions",
        description:""
    });

    y({
        url:'/api/Microsoft.AspNetCore.Hosting/HostingEnvironmentExtensions',
        title:"HostingEnvironmentExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_2',
        title:"InjectableMethodBuilder<T, T2>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.DependencyInjection/ApplicationServicesBuilder',
        title:"ApplicationServicesBuilder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Binding/PrivateSetterContractResolver',
        title:"PrivateSetterContractResolver",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/ValueExtensions',
        title:"ValueExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.DependencyInjection/ChildServicesBuilder',
        title:"ChildServicesBuilder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/AppDomainAssemblyProvider',
        title:"AppDomainAssemblyProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Build.Information/GitVersion',
        title:"GitVersion",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/CustomNodaConverters',
        title:"CustomNodaConverters",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/AppDomainAssemblyCandidateFinder',
        title:"AppDomainAssemblyCandidateFinder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_4',
        title:"InjectableMethodBuilder<T, T2, T3, T4>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.AspNetCore.Hosting/EnvironmentName',
        title:"EnvironmentName",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/UploadArtifactsOptions',
        title:"UploadArtifactsOptions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing.Docker/IEnsureContainerIsRunningContext',
        title:"IEnsureContainerIsRunningContext",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.DependencyInjection/ServicesBuilder',
        title:"ServicesBuilder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing.AzureStorageEmulator/AzureStorageEmulatorAutomation',
        title:"AzureStorageEmulatorAutomation",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Logging/LoggingBuilder',
        title:"LoggingBuilder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Build.Information/InformationProvider',
        title:"InformationProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_10',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_7',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Hosting/HostingEnvironment',
        title:"HostingEnvironment",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Encoding/CharMap',
        title:"Base32Url.CharMap",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/PackSettings',
        title:"Settings.PackSettings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_5',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Scanners/ConventionScannerExtensions',
        title:"ConventionScannerExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionComposerExtensions',
        title:"ConventionComposerExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Unions/UnionConverter',
        title:"UnionConverter",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/ReportTestResultsOptions',
        title:"ReportTestResultsOptions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/InstantPatternConverter',
        title:"InstantPatternConverter",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Binding/JsonBinder',
        title:"JsonBinder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Binding/BackingFieldValueProvider',
        title:"BackingFieldValueProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/Extensions',
        title:"Extensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_12',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/DefaultAssemblyCandidateFinder',
        title:"DefaultAssemblyCandidateFinder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/OffsetDateTimePatternConverter',
        title:"OffsetDateTimePatternConverter",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Encoding/Base64Url',
        title:"Base64Url",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionContext',
        title:"ConventionContext",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage.Converters/NodaMultiplePatternConverter_1',
        title:"NodaMultiplePatternConverter<T>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing/TestBase',
        title:"TestBase",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionComposer_3',
        title:"ConventionComposer<TContext, TContribution, TDelegate>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage/CloudTableProvider',
        title:"CloudTableProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/CodeCoverageType',
        title:"CodeCoverageType",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionBuilder_3',
        title:"ConventionBuilder<TBuilder, TConvention, TDelegate>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage/ComplexTableEntity',
        title:"ComplexTableEntity",
        description:""
    });

    y({
        url:'/api/System.Collections.Generic/DeconstructorExtensions',
        title:"DeconstructorExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing/AutoTestBase',
        title:"AutoTestBase",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/TypeInfoExtensions',
        title:"TypeInfoExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/DefaultAssemblyProvider',
        title:"DefaultAssemblyProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_9',
        title:"InjectableMethodBuilder<T, T2, T3, T4, T5, T6, T7, T8, T9>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder_3',
        title:"InjectableMethodBuilder<T, T2, T3>",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/Tfs',
        title:"Tfs",
        description:""
    });

    y({
        url:'/api/Microsoft.WindowsAzure.Storage.Table/AzureStorageExtensions',
        title:"AzureStorageExtensions",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/CoverageSettings',
        title:"Settings.CoverageSettings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/TfsCakeAliases',
        title:"TfsCakeAliases",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/TFBuildPullRequestInfo',
        title:"TFBuildPullRequestInfo",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Azure.Storage/AzureStorageSettings',
        title:"AzureStorageSettings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/PropertyGetter',
        title:"PropertyGetter",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/Settings',
        title:"Settings",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/InjectableMethodBuilder',
        title:"InjectableMethodBuilder",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Encoding/Base3264Encoding',
        title:"Base3264Encoding",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake.TfsTasks/ArtifactType',
        title:"ArtifactType",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Scanners/AggregateConventionScanner',
        title:"AggregateConventionScanner",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Cake/LocalCakeAliases',
        title:"LocalCakeAliases",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/Composer',
        title:"Composer",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions/ConventionComposerBase',
        title:"ConventionComposerBase",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Reflection/DependencyContextAssemblyProvider',
        title:"DependencyContextAssemblyProvider",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Scanners/BasicConventionScanner',
        title:"BasicConventionScanner",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Extensions.Testing/XunitLogger',
        title:"XunitLogger",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Conventions.Scanners/ConventionScannerBase',
        title:"ConventionScannerBase",
        description:""
    });

    y({
        url:'/api/Rocket.Surgery.Reflection.Extensions/PropertyDelegate',
        title:"PropertyDelegate",
        description:""
    });

    y({
        url:'/api/System.Linq/LinqExtensions',
        title:"LinqExtensions",
        description:""
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();
