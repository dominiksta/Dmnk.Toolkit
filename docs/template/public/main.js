import hljsRazor from 'https://unpkg.com/highlightjs-cshtml-razor@2.2.0/dist/cshtml-razor.es.min.js';

export default {
  configureHljs: (hljs) => {
    hljs.registerLanguage("cshtml", hljsRazor);
    hljs.registerLanguage("razor", hljsRazor);
    hljs.registerLanguage("cshtml+razor", hljsRazor);
    hljs.registerLanguage("html+razor", hljsRazor);
  },
}