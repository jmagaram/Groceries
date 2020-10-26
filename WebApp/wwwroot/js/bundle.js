(()=>{"use strict";const e=Symbol("defaultState"),t=Symbol("delegatesFocus"),s=Symbol("firstRender"),n=Symbol("focusTarget"),r=Symbol("hasDynamicTemplate"),o=Symbol("ids"),i=Symbol("nativeInternals"),a=Symbol("raiseChangeEvents"),l=Symbol("render"),c=Symbol("renderChanges"),u=Symbol("rendered"),d=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),g=Symbol("shadowRootMode"),m=Symbol("state"),b=Symbol("stateEffects"),f=Symbol("template"),y=Symbol("mousedownListener");function w(e,t){return"boolean"==typeof t?t:"string"==typeof t&&(""===t||e.toLowerCase()===t.toLowerCase())}function v(e){for(const t of k(e)){const e=t[n]||t,s=e;if(e instanceof HTMLElement&&e.tabIndex>=0&&!s.disabled&&!(e instanceof HTMLSlotElement))return e}return null}function x(e,t){let s=t;for(;s;){const t=s.assignedSlot||s.parentNode||s.host;if(t===e)return!0;s=t}return!1}function T(e){const t=L(e,(e=>e instanceof HTMLElement&&e.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&e.tabIndex>=0)),{value:s}=t.next();return s instanceof HTMLElement?s:null}function P(e,t){e[y]&&e.removeEventListener("mousedown",e[y]),t&&(e[y]=e=>{if(0!==e.button)return;const s=v(t[n]||t);s&&(s.focus(),e.preventDefault())},e.addEventListener("mousedown",e[y]))}function E(e,t){return Array.prototype.findIndex.call(e,(e=>e===t||x(e,t)))}function I(e,t){const s=t.composedPath()[0];return e===s||x(e,s)}function*k(e){e&&(yield e,yield*function*(e){let t=e;for(;t=t instanceof HTMLElement&&t.assignedSlot?t.assignedSlot:t instanceof ShadowRoot?t.host:t.parentNode,t;)yield t}(e))}function S(e,t,s){e.toggleAttribute(t,s),e[i]&&e[i].states&&e[i].states.toggle(t,s)}const C={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function*L(e,t){let s;if(t(e)&&(yield e),e instanceof HTMLElement&&e.shadowRoot)s=e.shadowRoot.children;else{const t=e instanceof HTMLSlotElement?e.assignedNodes({flatten:!0}):[];s=t.length>0?t:e.childNodes}if(s)for(let e=0;e<s.length;e++)yield*L(s[e],t)}const O=(e,...t)=>A.html(e,...t).content,A={html(e,...t){const s=document.createElement("template");return s.innerHTML=String.raw(e,...t),s}},D={tabindex:"tabIndex"},M={tabIndex:"tabindex"};function R(e){if(e===HTMLElement)return[];const t=Object.getPrototypeOf(e.prototype).constructor;let s=t.observedAttributes;s||(s=R(t));const n=Object.getOwnPropertyNames(e.prototype).filter((t=>{const s=Object.getOwnPropertyDescriptor(e.prototype,t);return s&&"function"==typeof s.set})).map((e=>function(e){let t=M[e];if(!t){const s=/([A-Z])/g;t=e.replace(s,"-$1").toLowerCase(),M[e]=t}return t}(e))).filter((e=>s.indexOf(e)<0));return s.concat(n)}const B=Symbol("state"),F=Symbol("raiseChangeEventsInNextRender"),j=Symbol("changedSinceLastRender");function H(e,t){const s={};for(const o in t)n=t[o],r=e[o],(n instanceof Date&&r instanceof Date?n.getTime()===r.getTime():n===r)||(s[o]=!0);var n,r;return s}const N=new Map,z=Symbol("shadowIdProxy"),W=Symbol("proxyElement"),V={get(e,t){const s=e[W][p];return s&&"string"==typeof t?s.getElementById(t):null}};function Y(e){let t=e[r]?void 0:N.get(e.constructor);if(void 0===t&&(t=e[f],t)){if(!(t instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${e.constructor.name} must return an HTMLTemplateElement.`;e[r]||N.set(e.constructor,t)}return t}const U=function(e){return class extends e{attributeChangedCallback(e,t,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(e,t,s),s!==t&&!this[d]){const t=function(e){let t=D[e];if(!t){const s=/-([a-z])/g;t=e.replace(s,(e=>e[1].toUpperCase())),D[e]=t}return t}(e);if(t in this){const n=C[e]?w(e,s):s;this[t]=n}}}static get observedAttributes(){return R(this)}}}(function(t){class n extends t{constructor(){super(),this[s]=void 0,this[a]=!1,this[j]=null,this[h](this[e])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[c]()}get[e](){return super[e]||{}}[l](e){super[l]&&super[l](e)}[c](){void 0===this[s]&&(this[s]=!0);const e=this[j];if(this[s]||e){const t=this[a];this[a]=this[F],this[d]=!0,this[l](e),this[d]=!1,this[j]=null,this[u](e),this[s]=!1,this[a]=t,this[F]=t}}[u](e){super[u]&&super[u](e)}async[h](e){this[d]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:t,changed:n}=function(e,t){const s=Object.assign({},e[B]),n={};let r=t;for(;;){const t=H(s,r);if(0===Object.keys(t).length)break;Object.assign(s,r),Object.assign(n,t),r=e[b](s,t)}return{state:s,changed:n}}(this,e);if(this[B]&&0===Object.keys(n).length)return;Object.freeze(t),this[B]=t,this[a]&&(this[F]=!0);const r=void 0===this[s]||null!==this[j];this[j]=Object.assign(this[j]||{},n),this.isConnected&&!r&&(await Promise.resolve(),this[c]())}get[m](){return this[B]}[b](e,t){return super[b]?super[b](e,t):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(n.prototype,"state",{get(){return this[m]}}),n}(function(e){return class extends e{get[o](){if(!this[z]){const e={[W]:this};this[z]=new Proxy(e,V)}return this[z]}[l](e){if(super[l]&&super[l](e),void 0===this[s]||this[s]){const e=Y(this);if(e){const s=this.attachShadow({delegatesFocus:this[t],mode:this[g]}),n=document.importNode(e.content,!0);s.append(n),this[p]=s}}}get[g](){return"open"}}}(HTMLElement))),G=new Map;function q(e){if("function"==typeof e){let t;try{t=new e}catch(s){if("TypeError"!==s.name)throw s;!function(e){let t;const s=e.name&&e.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const e=/([A-Z])/g;t=s[0].replace(e,((e,t,s)=>s>0?"-"+t:t)).toLowerCase()}else t="custom-element";let n,r=G.get(t)||0;for(;n=`${t}-${r}`,customElements.get(n);r++);customElements.define(n,e),G.set(t,r+1)}(e),t=new e}return t}return document.createElement(e)}function K(e,t){const s=e.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";return(e instanceof HTMLElement||e instanceof SVGElement)&&(t instanceof HTMLElement||t instanceof SVGElement)&&(Array.prototype.forEach.call(e.attributes,(e=>{t.getAttribute(e.name)||"class"===e.name||"style"===e.name||t.setAttribute(e.name,e.value)})),Array.prototype.forEach.call(e.classList,(e=>{t.classList.add(e)})),Array.prototype.forEach.call(e.style,(s=>{t.style[s]||(t.style[s]=e.style[s])}))),t.append(...e.childNodes),s.replaceChild(t,e),t}function Z(e,t){if("function"==typeof t&&e.constructor===t||"string"==typeof t&&e instanceof Element&&e.localName===t)return e;{const s=q(t);return K(e,s),s}}Symbol("applyElementData");const $=Symbol("checkSize"),J=Symbol("closestAvailableItemIndex"),Q=Symbol("contentSlot"),X=e,_=Symbol("defaultTabIndex"),ee=t,te=Symbol("effectEndTarget"),se=s,ne=n,re=Symbol("getItemText"),oe=Symbol("goDown"),ie=Symbol("goEnd"),ae=Symbol("goFirst"),le=Symbol("goLast"),ce=Symbol("goLeft"),ue=Symbol("goNext"),de=Symbol("goPrevious"),he=Symbol("goRight"),pe=Symbol("goStart"),ge=Symbol("goToItemWithPrefix"),me=Symbol("goUp"),be=r,fe=o,ye=Symbol("inputDelegate"),we=Symbol("itemsDelegate"),ve=Symbol("keydown"),xe=(Symbol("matchText"),Symbol("mouseenter")),Te=Symbol("mouseleave"),Pe=i,Ee=a,Ie=l,ke=c,Se=Symbol("renderDataToElement"),Ce=u,Le=d,Oe=Symbol("scrollTarget"),Ae=h,De=p,Me=g,Re=Symbol("startEffect"),Be=m,Fe=b,je=Symbol("swipeDown"),He=Symbol("swipeDownComplete"),Ne=Symbol("swipeLeft"),ze=Symbol("swipeLeftTransitionEnd"),We=Symbol("swipeRight"),Ve=Symbol("swipeRightTransitionEnd"),Ye=Symbol("swipeUp"),Ue=Symbol("swipeUpComplete"),Ge=Symbol("swipeStart"),qe=Symbol("swipeTarget"),Ke=Symbol("tap"),Ze=f,$e=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:$,closestAvailableItemIndex:J,contentSlot:Q,defaultState:X,defaultTabIndex:_,delegatesFocus:ee,effectEndTarget:te,firstRender:se,focusTarget:ne,getItemText:re,goDown:oe,goEnd:ie,goFirst:ae,goLast:le,goLeft:ce,goNext:ue,goPrevious:de,goRight:he,goStart:pe,goToItemWithPrefix:ge,goUp:me,hasDynamicTemplate:be,ids:fe,inputDelegate:ye,itemsDelegate:we,keydown:ve,mouseenter:xe,mouseleave:Te,nativeInternals:Pe,event,raiseChangeEvents:Ee,render:Ie,renderChanges:ke,renderDataToElement:Se,rendered:Ce,rendering:Le,scrollTarget:Oe,setState:Ae,shadowRoot:De,shadowRootMode:Me,startEffect:Re,state:Be,stateEffects:Fe,swipeDown:je,swipeDownComplete:He,swipeLeft:Ne,swipeLeftTransitionEnd:ze,swipeRight:We,swipeRightTransitionEnd:Ve,swipeUp:Ye,swipeUpComplete:Ue,swipeStart:Ge,swipeTarget:qe,tap:Ke,template:Ze,toggleSelectedFlag:$e}});const Je=document.createElement("div");Je.attachShadow({mode:"open",delegatesFocus:!0});const Qe=Je.shadowRoot.delegatesFocus;function Xe(e){if("selectedText"in e)return e.selectedText;if("value"in e&&"options"in e){const t=e.value,s=e.options.find((e=>e.value===t));return s?s.innerText:""}return"value"in e?e.value:e.innerText}function _e(e,t){const{ariaLabel:s,ariaLabelledby:n}=t,r=e.isConnected?e.getRootNode():null;let o=null;if(n&&r)o=n.split(" ").map((s=>{const n=r.getElementById(s);return n?n===e&&null!==t.value?t.selectedText:Xe(n):""})).join(" ");else if(s)o=s;else if(r){const t=e.id;if(t){const e=r.querySelector(`[for="${t}"]`);e instanceof HTMLElement&&(o=Xe(e))}if(null===o){const t=e.closest("label");t&&(o=Xe(t))}}return o&&(o=o.trim()),o}let et=!1;const tt=Symbol("focusVisibleChangedListener");function st(e){return class extends e{constructor(){super(),this.addEventListener("focusout",(e=>{Promise.resolve().then((()=>{const t=e.relatedTarget||document.activeElement,s=this===t,n=x(this,t);!s&&!n&&(this[Ae]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[tt]),this[tt]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[Be].focusVisible!==et&&this[Ae]({focusVisible:et}),this[tt]||(this[tt]=()=>{this[Ae]({focusVisible:et})},document.addEventListener("focusvisiblechange",this[tt]))}))}))}get[X](){return Object.assign(super[X]||{},{focusVisible:!1})}[Ie](e){if(super[Ie]&&super[Ie](e),e.focusVisible){const{focusVisible:e}=this[Be];this.toggleAttribute("focus-visible",e)}}get[Ze](){const e=super[Ze]||A.html``;return e.content.append(O`
        <style>
          :host {
            outline: none;
          }

          :host([focus-visible]:focus-within) {
            outline-color: Highlight; /* Firefox */
            outline-color: -webkit-focus-ring-color; /* All other browsers */
            outline-style: auto;
          }
        </style>
      `),e}}}function nt(e){if(et!==e){et=e;const t=new CustomEvent("focus-visible-changed",{detail:{focusVisible:et}});document.dispatchEvent(t);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:et}});document.dispatchEvent(s)}}function rt(e){return class extends e{get[ee](){return!0}focus(e){const t=this[ne];t&&t.focus(e)}get[ne](){return T(this[De])}}}window.addEventListener("keydown",(()=>{nt(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{nt(!1)}),{capture:!0});const ot=Symbol("extends"),it=Symbol("delegatedPropertySetters"),at={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},lt={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},ct=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],ut={abort:!0,change:!0,reset:!0},dt=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],ht=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],pt=rt(U);class gt extends pt{constructor(){super();!this[Pe]&&this.attachInternals&&(this[Pe]=this.attachInternals())}attributeChangedCallback(e,t,s){if(ht.indexOf(e)>=0){const t=Object.assign({},this[Be].innerAttributes,{[e]:s});this[Ae]({innerAttributes:t})}else super.attributeChangedCallback(e,t,s)}blur(){this.inner.blur()}get[X](){return Object.assign(super[X],{innerAttributes:{}})}get[_](){return at[this.extends]?0:-1}get extends(){return this.constructor[ot]}get inner(){const e=this[fe]&&this[fe].inner;return e||console.warn("Attempted to get an inner standard element before it was instantiated."),e}getInnerProperty(e){return this[Be][e]||this[De]&&this.inner[e]}static get observedAttributes(){return[...super.observedAttributes,...ht]}[Ie](e){super[Ie](e);const t=this.inner;if(this[se]&&((lt[this.extends]||[]).forEach((e=>{t.addEventListener(e,(()=>{const t=new Event(e,{bubbles:ut[e]||!1});this.dispatchEvent(t)}))})),"disabled"in t&&ct.forEach((e=>{this.addEventListener(e,(e=>{t.disabled&&e.stopImmediatePropagation()}))}))),e.tabIndex&&(t.tabIndex=this[Be].tabIndex),e.innerAttributes){const{innerAttributes:e}=this[Be];for(const s in e)mt(t,s,e[s])}this.constructor[it].forEach((s=>{if(e[s]){const e=this[Be][s];("selectionEnd"===s||"selectionStart"===s)&&null===e||(t[s]=e)}}))}[Ce](e){if(super[Ce](e),e.disabled){const{disabled:e}=this[Be];void 0!==e&&S(this,"disabled",e)}}setInnerProperty(e,t){this[Be][e]!==t&&this[Ae]({[e]:t})}get[Ze](){const e=dt.includes(this.extends)?"block":"inline-block";return A.html`
      <style>
        :host {
          display: ${e}
        }
        
        [part~="inner"] {
          box-sizing: border-box;
          height: 100%;
          width: 100%;
        }
      </style>
      <${this.extends} id="inner" part="inner">
        <slot></slot>
      </${this.extends}>
    `}static wrap(e){class t extends gt{}t[ot]=e;const s=document.createElement(e);return function(e,t){const s=Object.getOwnPropertyNames(t);e[it]=[],s.forEach((s=>{const n=Object.getOwnPropertyDescriptor(t,s);if(!n)return;const r=function(e,t){if("function"==typeof t.value){if("constructor"!==e)return function(e,t){return{configurable:t.configurable,enumerable:t.enumerable,value:function(...t){this.inner[e](...t)},writable:t.writable}}(e,t)}else if("function"==typeof t.get||"function"==typeof t.set)return function(e,t){const s={configurable:t.configurable,enumerable:t.enumerable};return t.get&&(s.get=function(){return this.getInnerProperty(e)}),t.set&&(s.set=function(t){this.setInnerProperty(e,t)}),t.writable&&(s.writable=t.writable),s}(e,t);return null}(s,n);r&&(Object.defineProperty(e.prototype,s,r),r.set&&e[it].push(s))}))}(t,Object.getPrototypeOf(s)),t}}function mt(e,t,s){C[t]?"string"==typeof s?e.setAttribute(t,""):null===s&&e.removeAttribute(t):null!=s?e.setAttribute(t,s.toString()):e.removeAttribute(t)}const bt=function(e){return class extends e{get[X](){return Object.assign(super[X]||{},{composeFocus:!Qe})}[Ie](e){super[Ie]&&super[Ie](e),this[se]&&this.addEventListener("mousedown",(e=>{if(this[Be].composeFocus&&0===e.button&&e.target instanceof Element){const t=v(e.target);t&&(t.focus(),e.preventDefault())}}))}}}(function(e){return class extends e{get ariaLabel(){return this[Be].ariaLabel}set ariaLabel(e){this[Be].removingAriaAttribute||this[Ae]({ariaLabel:e})}get ariaLabelledby(){return this[Be].ariaLabelledby}set ariaLabelledby(e){this[Be].removingAriaAttribute||this[Ae]({ariaLabelledby:e})}get[X](){return Object.assign(super[X]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[Ie](e){if(super[Ie]&&super[Ie](e),this[se]&&this.addEventListener("focus",(()=>{this[Ee]=!0;const e=_e(this,this[Be]);this[Ae]({inputLabel:e}),this[Ee]=!1})),e.inputLabel){const{inputLabel:e}=this[Be];e?this[ye].setAttribute("aria-label",e):this[ye].removeAttribute("aria-label")}}[Ce](e){super[Ce]&&super[Ce](e),this[se]&&(window.requestIdleCallback||setTimeout)((()=>{const e=_e(this,this[Be]);this[Ae]({inputLabel:e})}));const{ariaLabel:t,ariaLabelledby:s}=this[Be];e.ariaLabel&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",t),this[Ae]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),e.ariaLabelledby&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Ae]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),e.removingAriaAttribute&&this[Be].removingAriaAttribute&&this[Ae]({removingAriaAttribute:!1})}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};if(t.ariaLabel&&e.ariaLabel||t.selectedText&&e.ariaLabelledby&&this.matches(":focus-within")){const t=_e(this,e);Object.assign(s,{inputLabel:t})}return s}}}(st(gt.wrap("button")))),ft=class extends bt{get[X](){return Object.assign(super[X],{role:"button"})}get[ye](){return this[fe].inner}[Ke](){const e=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(e)}get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            display: inline-flex;
            outline: none;
            -webkit-tap-highlight-color: transparent;
            touch-action: manipulation;
          }

          [part~="inner"] {
            align-items: center;
            background: none;
            border: none;
            color: inherit;
            flex: 1;
            font: inherit;
            outline: none;
            padding: 0;
          }
        </style>
      `),e}},yt=Symbol("wrap");function wt(e){return class extends e{get arrowButtonOverlap(){return this[Be].arrowButtonOverlap}set arrowButtonOverlap(e){this[Ae]({arrowButtonOverlap:e})}get arrowButtonPartType(){return this[Be].arrowButtonPartType}set arrowButtonPartType(e){this[Ae]({arrowButtonPartType:e})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[de]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[ue]()}attributeChangedCallback(e,t,s){"arrow-button-overlap"===e?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===e?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(e,t,s)}get[X](){return Object.assign(super[X]||{},{arrowButtonOverlap:!0,arrowButtonPartType:ft,orientation:"horizontal",showArrowButtons:!0})}[Ie](e){if(e.arrowButtonPartType){const e=this[fe].arrowButtonPrevious;e instanceof HTMLElement&&P(e,null);const t=this[fe].arrowButtonNext;t instanceof HTMLElement&&P(t,null)}if(super[Ie]&&super[Ie](e),xt(this[De],this[Be],e),e.arrowButtonPartType){const e=this,t=this[fe].arrowButtonPrevious;t instanceof HTMLElement&&P(t,e);const s=vt(this,(()=>this.arrowButtonPrevious()));t.addEventListener("mousedown",s);const n=this[fe].arrowButtonNext;n instanceof HTMLElement&&P(n,e);const r=vt(this,(()=>this.arrowButtonNext()));n.addEventListener("mousedown",r)}const{arrowButtonOverlap:t,canGoNext:s,canGoPrevious:n,orientation:r,rightToLeft:o}=this[Be],i="vertical"===r,a=this[fe].arrowButtonPrevious,l=this[fe].arrowButtonNext;if(e.arrowButtonOverlap||e.orientation||e.rightToLeft){this[fe].arrowDirection.style.flexDirection=i?"column":"row";const e={bottom:null,left:null,right:null,top:null};let s,n;t?Object.assign(e,{position:"absolute","z-index":1}):Object.assign(e,{position:null,"z-index":null}),t&&(i?(Object.assign(e,{left:0,right:0}),s={top:0},n={bottom:0}):(Object.assign(e,{bottom:0,top:0}),o?(s={right:0},n={left:0}):(s={left:0},n={right:0}))),Object.assign(a.style,e,s),Object.assign(l.style,e,n)}if(e.canGoNext&&null!==s&&(l.disabled=!s),e.canGoPrevious&&null!==n&&(a.disabled=!n),e.showArrowButtons){const e=this[Be].showArrowButtons?null:"none";a.style.display=e,l.style.display=e}}get showArrowButtons(){return this[Be].showArrowButtons}set showArrowButtons(e){this[Ae]({showArrowButtons:e})}[yt](e){const t=O`
        <div
          id="arrowDirection"
          role="none"
          style="display: flex; flex: 1; overflow: hidden; position: relative;"
        >
          <div
            id="arrowButtonPrevious"
            part="arrow-button arrow-button-previous"
            exportparts="inner:arrow-button-inner"
            class="arrowButton"
            aria-hidden="true"
            tabindex="-1"
          >
            <slot name="arrowButtonPrevious"></slot>
          </div>
          <div
            id="arrowDirectionContainer"
            role="none"
            style="flex: 1; overflow: hidden; position: relative;"
          ></div>
          <div
            id="arrowButtonNext"
            part="arrow-button arrow-button-next"
            exportparts="inner:arrow-button-inner"
            class="arrowButton"
            aria-hidden="true"
            tabindex="-1"
          >
            <slot name="arrowButtonNext"></slot>
          </div>
        </div>
      `;xt(t,this[Be]);const s=t.getElementById("arrowDirectionContainer");s&&(e.replaceWith(t),s.append(e))}}}function vt(e,t){return async function(s){0===s.button&&(e[Ee]=!0,t()&&s.stopPropagation(),await Promise.resolve(),e[Ee]=!1)}}function xt(e,t,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=t,n=e.getElementById("arrowButtonPrevious");n&&Z(n,s);const r=e.getElementById("arrowButtonNext");r&&Z(r,s)}}wt.wrap=yt;const Tt=wt,Pt={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},Et=864e5;function It(e,t){const s=e.includes("-ca-")?"":"-ca-gregory",n=e.includes("-nu-")?"":"-nu-latn",r=`${e}${s||n?"-u":""}${s}${n}`;return new Intl.DateTimeFormat(r,t)}function kt(e,t){return null===e&&null===t||null!==e&&null!==t&&e.getTime()===t.getTime()}function St(e,t){const s=Ct(t);return(e.getDay()-s+7)%7}function Ct(e){const t=Ht(e),s=Pt.firstDay[t];return void 0!==s?s:Pt.firstDay["001"]}function Lt(e){const t=Ot(e);return t.setDate(1),t}function Ot(e){const t=new Date(e.getTime());return t.setHours(0),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function At(e){const t=new Date(e.getTime());return t.setHours(12),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Dt(e,t){const s=At(e);return s.setDate(s.getDate()+t),jt(e,s),s}function Mt(e,t){const s=At(e);return s.setMonth(e.getMonth()+t),jt(e,s),s}function Rt(){return Ot(new Date)}function Bt(e){const t=Ht(e),s=Pt.weekendEnd[t];return void 0!==s?s:Pt.weekendEnd["001"]}function Ft(e){const t=Ht(e),s=Pt.weekendStart[t];return void 0!==s?s:Pt.weekendStart["001"]}function jt(e,t){t.setHours(e.getHours()),t.setMinutes(e.getMinutes()),t.setSeconds(e.getSeconds()),t.setMilliseconds(e.getMilliseconds())}function Ht(e){const t=e?e.split("-"):null;return t?t[1]:"001"}function Nt(e){return class extends e{attributeChangedCallback(e,t,s){"date"===e?this.date=new Date(s):super.attributeChangedCallback(e,t,s)}get date(){return this[Be].date}set date(e){kt(e,this[Be].date)||this[Ae]({date:e})}get[X](){return Object.assign(super[X]||{},{date:null,locale:navigator.language})}get locale(){return this[Be].locale}set locale(e){this[Ae]({locale:e})}[Ce](e){if(super[Ce]&&super[Ce](e),e.date&&this[Ee]){const e=this[Be].date,t=new CustomEvent("date-changed",{bubbles:!0,detail:{date:e}});this.dispatchEvent(t);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:e}});this.dispatchEvent(s)}}}}function zt(e){return class extends e{constructor(){super();!this[Pe]&&this.attachInternals&&(this[Pe]=this.attachInternals())}get[X](){return Object.assign(super[X]||{},{selected:!1})}[Ie](e){if(super[Ie](e),e.selected){const{selected:e}=this[Be];S(this,"selected",e)}}[Ce](e){if(super[Ce]&&super[Ce](e),e.selected){const{selected:e}=this[Be],t=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(s)}}get selected(){return this[Be].selected}set selected(e){this[Ae]({selected:e})}}}const Wt=Nt(zt(U)),Vt=class extends Wt{get[X](){return Object.assign(super[X],{date:Rt(),outsideRange:!1})}[Ie](e){super[Ie](e);const{date:t}=this[Be];if(e.date){const e=Rt(),s=t.getDay(),n=t.getDate(),r=Dt(t,1),o=Math.round(t.getTime()-e.getTime())/Et;S(this,"alternate-month",Math.abs(t.getMonth()-e.getMonth())%2==1),S(this,"first-day-of-month",1===n),S(this,"first-week",n<=7),S(this,"future",t>e),S(this,"last-day-of-month",t.getMonth()!==r.getMonth()),S(this,"past",t<e),S(this,"sunday",0===s),S(this,"monday",1===s),S(this,"tuesday",2===s),S(this,"wednesday",3===s),S(this,"thursday",4===s),S(this,"friday",5===s),S(this,"saturday",6===s),S(this,"today",0===o),this[fe].day.textContent=n.toString()}if(e.date||e.locale){const e=t.getDay(),{locale:s}=this[Be],n=e===Ft(s)||e===Bt(s);S(this,"weekday",!n),S(this,"weekend",n)}e.outsideRange&&S(this,"outside-range",this[Be].outsideRange)}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Ae]({outsideRange:e})}get[Ze](){return A.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Yt=zt(ft),Ut=Nt(class extends Yt{}),Gt=class extends Ut{get[X](){return Object.assign(super[X],{date:Rt(),dayPartType:Vt,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Ae]({dayPartType:e})}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Ae]({outsideRange:e})}[Ie](e){if(super[Ie](e),e.dayPartType){const{dayPartType:e}=this[Be];Z(this[fe].day,e)}const t=this[fe].day;(e.dayPartType||e.date)&&(t.date=this[Be].date),(e.dayPartType||e.locale)&&(t.locale=this[Be].locale),(e.dayPartType||e.outsideRange)&&(t.outsideRange=this[Be].outsideRange),(e.dayPartType||e.selected)&&(t.selected=this[Be].selected)}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");if(t){const e=q(this[Be].dayPartType);e.id="day",t.replaceWith(e)}return e.content.append(O`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),e}},qt=class extends U{get[X](){return Object.assign(super[X],{format:"short",locale:navigator.language})}get format(){return this[Be].format}set format(e){this[Ae]({format:e})}get locale(){return this[Be].locale}set locale(e){this[Ae]({locale:e})}[Ie](e){if(super[Ie](e),e.format||e.locale){const{format:e,locale:t}=this[Be],s=It(t,{weekday:e}),n=Ct(t),r=Ft(t),o=Bt(t),i=new Date(2017,0,1),a=this[De].querySelectorAll('[part~="day-name"]');for(let e=0;e<=6;e++){const t=(n+e)%7;i.setDate(t+1);const l=t===r||t===o,c=a[e];c.toggleAttribute("weekday",!l),c.toggleAttribute("weekend",l),c.textContent=s.format(i)}}}get[Ze](){return A.html`
      <style>
        :host {
          direction: ltr;
          display: inline-grid;
          grid-template-columns: repeat(7, 1fr);
        }
      </style>

      <div id="day0" part="day-name"></div>
      <div id="day1" part="day-name"></div>
      <div id="day2" part="day-name"></div>
      <div id="day3" part="day-name"></div>
      <div id="day4" part="day-name"></div>
      <div id="day5" part="day-name"></div>
      <div id="day6" part="day-name"></div>
    `}},Kt=Nt(U),Zt=class extends Kt{attributeChangedCallback(e,t,s){"start-date"===e?this.startDate=new Date(s):super.attributeChangedCallback(e,t,s)}dayElementForDate(e){return(this.days||[]).find((t=>kt(t.date,e)))}get dayCount(){return this[Be].dayCount}set dayCount(e){this[Ae]({dayCount:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Ae]({dayPartType:e})}get days(){return this[Be].days}get[X](){const e=Rt();return Object.assign(super[X],{date:e,dayCount:1,dayPartType:Vt,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:e})}[Ie](e){if(super[Ie](e),e.days&&function(e,t){const s=[...t],n=e.childNodes.length,r=s.length,o=Math.max(n,r);for(let t=0;t<o;t++){const o=e.childNodes[t],i=s[t];t>=n?e.append(i):t>=r?e.removeChild(e.childNodes[r]):o!==i&&(s.indexOf(o,t)>=t?e.insertBefore(i,o):e.replaceChild(i,o))}}(this[fe].dayContainer,this[Be].days),e.date||e.locale||e.showSelectedDay){const e=this[Be].showSelectedDay,{date:t}=this[Be],s=t.getDate(),n=t.getMonth(),r=t.getFullYear();(this.days||[]).forEach((t=>{const o=t.date,i=e&&o.getDate()===s&&o.getMonth()===n&&o.getFullYear()===r;t.toggleAttribute("selected",i)}))}if(e.dayCount||e.startDate){const{dayCount:e,startDate:t}=this[Be],s=Dt(t,e);(this[Be].days||[]).forEach((e=>{if("outsideRange"in e){const n=e.date.getTime(),r=n<t.getTime()||n>=s.getTime();e.outsideRange=r}}))}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Ae]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Ae]({showSelectedDay:e})}get startDate(){return this[Be].startDate}set startDate(e){kt(this[Be].startDate,e)||this[Ae]({startDate:e})}[Fe](e,t){const s=super[Fe](e,t);if(t.dayCount||t.dayPartType||t.locale||t.showCompleteWeeks||t.startDate){const n=function(e,t){const{dayCount:s,dayPartType:n,locale:r,showCompleteWeeks:o,startDate:i}=e,a=o?function(e,t){return Ot(Dt(e,-St(e,t)))}(i,r):Ot(i);let l;if(o){c=a,u=function(e,t){return Ot(Dt(e,6-St(e,t)))}(Dt(i,s-1),r),l=Math.round((u.getTime()-c.getTime())/Et)+1}else l=s;var c,u;let d=e.days?e.days.slice():[],h=a;for(let e=0;e<l;e++){const s=t||e>=d.length,o=s?q(n):d[e];o.date=new Date(h.getTime()),o.locale=r,"part"in o&&(o.part="day"),o.style.gridColumnStart="",s&&(d[e]=o),h=Dt(h,1)}l<d.length&&(d=d.slice(0,l));const p=d[0];if(p&&!o){const t=St(p.date,e.locale);p.style.gridColumnStart=t+1}return Object.freeze(d),d}(e,t.dayPartType);Object.assign(s,{days:n})}return s}get[Ze](){return A.html`
      <style>
        :host {
          display: inline-block;
        }

        [part~="day-container"] {
          direction: ltr;
          display: grid;
          grid-template-columns: repeat(7, 1fr);
        }
      </style>

      <div id="dayContainer" part="day-container"></div>
    `}},$t=Nt(U),Jt=class extends $t{get[X](){return Object.assign(super[X],{date:Rt(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Ae]({monthFormat:e})}[Ie](e){if(super[Ie](e),e.date||e.locale||e.monthFormat||e.yearFormat){const{date:e,locale:t,monthFormat:s,yearFormat:n}=this[Be],r={};s&&(r.month=s),n&&(r.year=n);const o=It(t,r);this[fe].formatted.textContent=o.format(e)}}get[Ze](){return A.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Ae]({yearFormat:e})}},Qt=Nt(U);function Xt(e,t,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=t,n=e.getElementById("dayNamesHeader");n&&Z(n,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=t,n=e.getElementById("monthYearHeader");n&&Z(n,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=t,n=e.getElementById("monthDays");n&&Z(n,s)}}function _t(e){return class extends e{[oe](){if(super[oe])return super[oe]()}[ie](){if(super[ie])return super[ie]()}[ce](){if(super[ce])return super[ce]()}[he](){if(super[he])return super[he]()}[pe](){if(super[pe])return super[pe]()}[me](){if(super[me])return super[me]()}[ve](e){let t=!1;const s=this[Be].orientation||"both",n="horizontal"===s||"both"===s,r="vertical"===s||"both"===s;switch(e.key){case"ArrowDown":r&&(t=e.altKey?this[ie]():this[oe]());break;case"ArrowLeft":!n||e.metaKey||e.altKey||(t=this[ce]());break;case"ArrowRight":!n||e.metaKey||e.altKey||(t=this[he]());break;case"ArrowUp":r&&(t=e.altKey?this[pe]():this[me]());break;case"End":t=this[ie]();break;case"Home":t=this[pe]()}return t||super[ve]&&super[ve](e)||!1}}}function es(e){return class extends e{constructor(){super(),this.addEventListener("keydown",(async e=>{this[Ee]=!0,this[Be].focusVisible||this[Ae]({focusVisible:!0}),this[ve](e)&&(e.preventDefault(),e.stopImmediatePropagation()),await Promise.resolve(),this[Ee]=!1}))}attributeChangedCallback(e,t,s){if("tabindex"===e){let e;null===s?e=-1:(e=Number(s),isNaN(e)&&(e=this[_]?this[_]:0)),this.tabIndex=e}else super.attributeChangedCallback(e,t,s)}get[X](){const e=this[ee]?-1:0;return Object.assign(super[X]||{},{tabIndex:e})}[ve](e){return!!super[ve]&&super[ve](e)}[Ie](e){super[Ie]&&super[Ie](e),e.tabIndex&&(this.tabIndex=this[Be].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(e){super.tabIndex!==e&&(super.tabIndex=e),this[Le]||this[Ae]({tabIndex:e})}}}function ts(e){return class extends e{connectedCallback(){const e="rtl"===getComputedStyle(this).direction;this[Ae]({rightToLeft:e}),super.connectedCallback()}}}const ss=Tt(Nt(st(function(e){return class extends e{constructor(){super();!this[Pe]&&this.attachInternals&&(this[Pe]=this.attachInternals())}checkValidity(){return this[Pe].checkValidity()}get[X](){return Object.assign(super[X]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[Pe]}static get formAssociated(){return!0}get form(){return this[Pe].form}get name(){return this[Be]?this[Be].name:""}set name(t){"name"in e.prototype&&(super.name=t),this[Ae]({name:t})}[Ie](e){if(super[Ie]&&super[Ie](e),e.name&&this.setAttribute("name",this[Be].name),this[Pe]&&this[Pe].setValidity&&(e.valid||e.validationMessage)){const{valid:e,validationMessage:t}=this[Be];e?this[Pe].setValidity({}):this[Pe].setValidity({customError:!0},t)}}[Ce](e){super[Ce]&&super[Ce](e),e.value&&this[Pe]&&this[Pe].setFormValue(this[Be].value,this[Be])}reportValidity(){return this[Pe].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[Be].validationMessage}get validity(){return this[Pe].validity}get willValidate(){return this[Pe].willValidate}}}(_t(es(ts(class extends Qt{dayElementForDate(e){const t=this[fe].monthDays;return t&&"dayElementForDate"in t&&t.dayElementForDate(e)}get dayNamesHeaderPartType(){return this[Be].dayNamesHeaderPartType}set dayNamesHeaderPartType(e){this[Ae]({dayNamesHeaderPartType:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Ae]({dayPartType:e})}get days(){return this[De]?this[fe].monthDays.days:[]}get daysOfWeekFormat(){return this[Be].daysOfWeekFormat}set daysOfWeekFormat(e){this[Ae]({daysOfWeekFormat:e})}get[X](){return Object.assign(super[X],{date:Rt(),dayNamesHeaderPartType:qt,dayPartType:Vt,daysOfWeekFormat:"short",monthDaysPartType:Zt,monthFormat:"long",monthYearHeaderPartType:Jt,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Ae]({monthFormat:e})}get monthDaysPartType(){return this[Be].monthDaysPartType}set monthDaysPartType(e){this[Ae]({monthDaysPartType:e})}get monthYearHeaderPartType(){return this[Be].monthYearHeaderPartType}set monthYearHeaderPartType(e){this[Ae]({monthYearHeaderPartType:e})}[Ie](e){if(super[Ie](e),Xt(this[De],this[Be],e),(e.dayPartType||e.monthDaysPartType)&&(this[fe].monthDays.dayPartType=this[Be].dayPartType),e.locale||e.monthDaysPartType||e.monthYearHeaderPartType||e.dayNamesHeaderPartType){const e=this[Be].locale;this[fe].monthDays.locale=e,this[fe].monthYearHeader.locale=e,this[fe].dayNamesHeader.locale=e}if(e.date||e.monthDaysPartType){const{date:e}=this[Be];if(e){const t=Lt(e),s=function(e){const t=Lt(e);return t.setMonth(t.getMonth()+1),t.setDate(t.getDate()-1),t}(e).getDate();Object.assign(this[fe].monthDays,{date:e,dayCount:s,startDate:t}),this[fe].monthYearHeader.date=Lt(e)}}if(e.daysOfWeekFormat||e.dayNamesHeaderPartType){const{daysOfWeekFormat:e}=this[Be];this[fe].dayNamesHeader.format=e}if(e.showCompleteWeeks||e.monthDaysPartType){const{showCompleteWeeks:e}=this[Be];this[fe].monthDays.showCompleteWeeks=e}if(e.showSelectedDay||e.monthDaysPartType){const{showSelectedDay:e}=this[Be];this[fe].monthDays.showSelectedDay=e}if(e.monthFormat||e.monthYearHeaderPartType){const{monthFormat:e}=this[Be];this[fe].monthYearHeader.monthFormat=e}if(e.yearFormat||e.monthYearHeaderPartType){const{yearFormat:e}=this[Be];this[fe].monthYearHeader.yearFormat=e}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Ae]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Ae]({showSelectedDay:e})}get[Ze](){const e=A.html`
      <style>
        :host {
          display: inline-block;
        }

        [part~="month-year-header"] {
          display: block;
        }

        [part~="day-names-header"] {
          display: grid;
        }

        [part~="month-days"] {
          display: block;
        }
      </style>

      <div id="monthYearHeader" part="month-year-header"></div>
      <div
        id="dayNamesHeader"
        part="day-names-header"
        exportparts="day-name"
        format="short"
      ></div>
      <div id="monthDays" part="month-days" exportparts="day"></div>
    `;return Xt(e.content,this[Be]),e}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Ae]({yearFormat:e})}}))))))),ns=class extends ss{constructor(){super(),this.addEventListener("mousedown",(e=>{if(0!==e.button)return;this[Ee]=!0;const t=e.composedPath()[0];if(t instanceof Node){const e=this.days,s=e[E(e,t)];s&&(this.date=s.date)}this[Ee]=!1})),P(this,this)}arrowButtonNext(){const e=this[Be].date||Rt();return this[Ae]({date:Mt(e,1)}),!0}arrowButtonPrevious(){const e=this[Be].date||Rt();return this[Ae]({date:Mt(e,-1)}),!0}get[X](){return Object.assign(super[X],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Rt(),dayPartType:Gt,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[ve](e){let t=!1;switch(e.key){case"Home":this[Ae]({date:Rt()}),t=!0;break;case"PageDown":this[Ae]({date:Mt(this[Be].date,1)}),t=!0;break;case"PageUp":this[Ae]({date:Mt(this[Be].date,-1)}),t=!0}return t||super[ve]&&super[ve](e)}[oe](){return super[oe]&&super[oe](),this[Ae]({date:Dt(this[Be].date,7)}),!0}[ce](){return super[ce]&&super[ce](),this[Ae]({date:Dt(this[Be].date,-1)}),!0}[he](){return super[he]&&super[he](),this[Ae]({date:Dt(this[Be].date,1)}),!0}[me](){return super[me]&&super[me](),this[Ae]({date:Dt(this[Be].date,-7)}),!0}[Fe](e,t){const s=super[Fe](e,t);return t.date&&Object.assign(s,{value:e.date?e.date.toString():""}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("#monthYearHeader");this[Tt.wrap](t);const s=A.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return e.content.append(s.content),e}get value(){return this.date}set value(e){this.date=e}},rs=new Set;function os(e){return class extends e{attributeChangedCallback(e,t,s){if("dark"===e){const t=w(e,s);this.dark!==t&&(this.dark=t)}else super.attributeChangedCallback(e,t,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),rs.delete(this)}get dark(){return this[Be].dark}set dark(e){this[Ae]({dark:e})}get[X](){return Object.assign(super[X]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[Be].detectDarkMode}set detectDarkMode(e){"auto"!==e&&"off"!==e||this[Ae]({detectDarkMode:e})}[Ie](e){if(super[Ie]&&super[Ie](e),e.dark){const{dark:e}=this[Be];S(this,"dark",e)}}[Ce](e){if(super[Ce]&&super[Ce](e),e.detectDarkMode){const{detectDarkMode:e}=this[Be];"auto"===e?(rs.add(this),is(this)):rs.delete(this)}}}}function is(e){const t=function(e){const t=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(e);return t?{r:t[1],g:t[2],b:t[3]}:null}(as(e));if(t){const s=function(e){const t=e.r/255,s=e.g/255,n=e.b/255,r=Math.max(t,s,n),o=Math.min(t,s,n);let i=0,a=0,l=(r+o)/2;const c=r-o;if(0!==c){switch(a=l>.5?c/(2-c):c/(r+o),r){case t:i=(s-n)/c+(s<n?6:0);break;case s:i=(n-t)/c+2;break;case n:i=(t-s)/c+4}i/=6}return{h:i,s:a,l}}(t).l<.5;e[Ae]({dark:s})}}function as(e){const t="rgb(255,255,255)";if(e instanceof Document)return t;const s=getComputedStyle(e).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(e.assignedSlot)return as(e.assignedSlot);const n=e.parentNode;return n instanceof ShadowRoot?as(n.host):n instanceof Element?as(n):t}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{rs.forEach((e=>{is(e)}))}));class ls extends(function(e){return class extends e{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host([disabled]) ::slotted(*) {
            opacity: 0.5;
          }

          [part~="inner"] {
            display: inline-flex;
            justify-content: center;
            margin: 0;
            position: relative;
          }
        </style>
      `),e}}}(ft)){}const cs=ls,us=os(cs),ds=class extends us{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            color: rgba(0, 0, 0, 0.7);
          }

          :host(:not([disabled]):hover) {
            background: rgba(0, 0, 0, 0.2);
            color: rgba(0, 0, 0, 0.8);
            cursor: pointer;
          }

          :host([disabled]) {
            color: rgba(0, 0, 0, 0.3);
          }

          [part~="inner"] {
            fill: currentcolor;
          }

          :host([dark]) {
            color: rgba(255, 255, 255, 0.7);
          }

          :host([dark]:not([disabled]):hover) {
            background: rgba(255, 255, 255, 0.2);
            color: rgba(255, 255, 255, 0.8);
          }

          :host([dark][disabled]) {
            color: rgba(255, 255, 255, 0.3);
          }
        </style>
      `),e}},hs=class extends Vt{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            padding: 0.3em;
            text-align: right;
          }

          :host([weekend]) {
            color: gray;
          }

          :host([outside-range]) {
            color: lightgray;
          }

          :host([today]) {
            color: darkred;
            font-weight: bold;
          }

          :host([selected]) {
            background: #ddd;
          }
        </style>
      `),e}},ps=class extends Gt{get[X](){return Object.assign(super[X],{dayPartType:hs})}get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            border: 1px solid transparent;
          }

          :host(:hover) {
            border-color: gray;
          }

          :host([selected]) {
            background: #ddd;
          }
        </style>
      `),e}},gs=class extends qt{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            font-size: smaller;
          }

          [part~="day-name"] {
            padding: 0.3em;
            text-align: center;
            white-space: nowrap;
          }

          [weekend] {
            color: gray;
          }
        </style>
      `),e}},ms=class extends Jt{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),e}};class bs extends(os(function(e){return class extends e{get[X](){return Object.assign(super[X]||{},{arrowButtonPartType:ds})}[Ie](e){if(super[Ie](e),e.orientation||e.rightToLeft){const{orientation:e,rightToLeft:t}=this[Be],s="vertical"===e?"rotate(90deg)":t?"rotateZ(180deg)":"";this[fe].arrowIconPrevious&&(this[fe].arrowIconPrevious.style.transform=s),this[fe].arrowIconNext&&(this[fe].arrowIconNext.style.transform=s)}if(e.dark){const{dark:e}=this[Be],t=this[fe].arrowButtonPrevious,s=this[fe].arrowButtonNext;"dark"in t&&(t.dark=e),"dark"in s&&(s.dark=e)}}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="arrowButtonPrevious"]');t&&t.append(O`
            <svg
              id="arrowIconPrevious"
              part="arrow-icon arrow-icon-previous"
              viewBox="0 0 24 24"
              preserveAspectRatio="xMidYMid meet"
              style="fill: currentColor; height: 1em; width: 1em;"
            >
              <g>
                <path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path>
              </g>
            </svg>
          `);const s=e.content.querySelector('slot[name="arrowButtonNext"]');return s&&s.append(O`
            <svg
              id="arrowIconNext"
              part="arrow-icon arrow-icon-next"
              viewBox="0 0 24 24"
              preserveAspectRatio="xMidYMid meet"
              style="fill: currentColor; height: 1em; width: 1em;"
            >
              <g>
                <path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path>
              </g>
            </svg>
          `),e}}}(ns))){get[X](){return Object.assign(super[X],{dayNamesHeaderPartType:gs,dayPartType:ps,monthYearHeaderPartType:ms})}}const fs=bs;customElements.define("elix-calendar-month-navigator",class extends fs{}),Symbol("generatedId");const ys={a:"link",article:"region",button:"button",h1:"sectionhead",h2:"sectionhead",h3:"sectionhead",h4:"sectionhead",h5:"sectionhead",h6:"sectionhead",hr:"sectionhead",iframe:"region",link:"link",menu:"menu",ol:"list",option:"option",output:"liveregion",progress:"progressbar",select:"select",table:"table",td:"td",textarea:"textbox",th:"th",ul:"list"};function ws(e){const t=e[De],s=t&&t.querySelector("slot:not([name])");return s&&s.parentNode instanceof Element&&function(e){for(const t of k(e))if(t instanceof HTMLElement&&vs(t))return t;return null}(s.parentNode)||e}function vs(e){const t=getComputedStyle(e),s=t.overflowX,n=t.overflowY;return"scroll"===s||"auto"===s||"scroll"===n||"auto"===n}function xs(e,t,s){const n=e[J](e[Be],{direction:s,index:t});if(n<0)return!1;const r=e[Be].currentIndex!==n;return r&&e[Ae]({currentIndex:n}),r}const Ts=["applet","basefont","embed","font","frame","frameset","isindex","keygen","link","multicol","nextid","noscript","object","param","script","style","template","noembed"];function Ps(e,t,s){const n=e[Be].items,r=s?0:n.length-1,o=s?n.length:0,i=s?1:-1;let a,l,c=null;const{availableItemFlags:u}=e[Be];for(a=r;a!==o;a+=i)if((!u||u[a])&&(l=n[a].getBoundingClientRect(),l.top<=t&&t<=l.bottom)){c=n[a];break}if(!c||!l)return null;const d=getComputedStyle(c),h=d.paddingTop?parseFloat(d.paddingTop):0,p=d.paddingBottom?parseFloat(d.paddingBottom):0,g=l.top+h,m=g+c.clientHeight-h-p;return s&&g<=t||!s&&m>=t?a:a-i}function Es(e,t){const s=e[Be].items,n=e[Be].currentIndex,r=e[Oe].getBoundingClientRect(),o=Ps(e,t?r.bottom:r.top,t);let i;if(o&&n===o){const r=s[n].getBoundingClientRect(),o=e[Oe].clientHeight;i=Ps(e,t?r.bottom+o:r.top-o,t)}else i=o;if(!i){const n=t?s.length-1:0;i=e[J]?e[J](e[Be],{direction:t?-1:1,index:n}):n}const a=i!==n;if(a){const t=e[Ee];e[Ee]=!0,e[Ae]({currentIndex:i}),e[Ee]=t}return a}const Is=Symbol("typedPrefix"),ks=Symbol("prefixTimeout");function Ss(e){const t=e;t[ks]&&(clearTimeout(t[ks]),t[ks]=!1)}function Cs(e){e[Is]="",Ss(e)}function Ls(e){Ss(e),e[ks]=setTimeout((()=>{Cs(e)}),1e3)}function Os(e){return class extends e{get[Q](){const e=this[De]&&this[De].querySelector("slot:not([name])");return this[De]&&e||console.warn(`SlotContentMixin expects ${this.constructor.name} to define a shadow tree that includes a default (unnamed) slot.\nSee https://elix.org/documentation/SlotContentMixin.`),e}get[X](){return Object.assign(super[X]||{},{content:null})}[Ce](e){if(super[Ce]&&super[Ce](e),this[se]){const e=this[Q];e&&e.addEventListener("slotchange",(async()=>{this[Ee]=!0;const t=e.assignedNodes({flatten:!0});Object.freeze(t),this[Ae]({content:t}),await Promise.resolve(),this[Ee]=!1}))}}}}const As=function(e){return class extends e{get[X](){const e=super[X];return Object.assign(e,{itemRole:e.itemRole||"menuitem",role:e.role||"menu"})}get itemRole(){return this[Be].itemRole}set itemRole(e){this[Ae]({itemRole:e})}[Ie](e){super[Ie]&&super[Ie](e);const t=this[Be].items;if((e.items||e.itemRole)&&t){const{itemRole:e}=this[Be];t.forEach((t=>{e===ys[t.localName]?t.removeAttribute("role"):t.setAttribute("role",e)}))}if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[Le]||this[Ae]({role:e})}}}(function(e){return class extends e{attributeChangedCallback(e,t,s){if("current-index"===e)this.currentIndex=Number(s);else if("current-item-required"===e){const t=w(e,s);this.currentItemRequired!==t&&(this.currentItemRequired=t)}else if("cursor-operations-wrap"===e){const t=w(e,s);this.cursorOperationsWrap!==t&&(this.cursorOperationsWrap=t)}else super.attributeChangedCallback(e,t,s)}get currentIndex(){const{items:e,currentIndex:t}=this[Be];return e&&e.length>0?t:-1}set currentIndex(e){isNaN(e)||this[Ae]({currentIndex:e})}get currentItem(){const{items:e,currentIndex:t}=this[Be];return e&&e[t]}set currentItem(e){const{items:t}=this[Be];if(!t)return;const s=t.indexOf(e);s>=0&&this[Ae]({currentIndex:s})}get currentItemRequired(){return this[Be].currentItemRequired}set currentItemRequired(e){this[Ae]({currentItemRequired:e})}get cursorOperationsWrap(){return this[Be].cursorOperationsWrap}set cursorOperationsWrap(e){this[Ae]({cursorOperationsWrap:e})}goFirst(){return super.goFirst&&super.goFirst(),this[ae]()}goLast(){return super.goLast&&super.goLast(),this[le]()}goNext(){return super.goNext&&super.goNext(),this[ue]()}goPrevious(){return super.goPrevious&&super.goPrevious(),this[de]()}[Ce](e){if(super[Ce]&&super[Ce](e),e.currentIndex&&this[Ee]){const{currentIndex:e}=this[Be],t=new CustomEvent("current-index-changed",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("currentindexchange",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(s)}}}}(function(e){return class extends e{[Ce](e){super[Ce]&&super[Ce](e),e.currentItem&&this.scrollCurrentItemIntoView()}scrollCurrentItemIntoView(){super.scrollCurrentItemIntoView&&super.scrollCurrentItemIntoView();const{currentItem:e,items:t}=this[Be];if(!e||!t)return;const s=this[Oe].getBoundingClientRect(),n=e.getBoundingClientRect(),r=n.bottom-s.bottom,o=n.left-s.left,i=n.right-s.right,a=n.top-s.top,l=this[Be].orientation||"both";"horizontal"!==l&&"both"!==l||(i>0?this[Oe].scrollLeft+=i:o<0&&(this[Oe].scrollLeft+=Math.ceil(o))),"vertical"!==l&&"both"!==l||(r>0?this[Oe].scrollTop+=r:a<0&&(this[Oe].scrollTop+=Math.ceil(a)))}get[Oe](){return super[Oe]||ws(this)}}}(rt(function(e){return class extends e{get[X](){return Object.assign(super[X]||{},{canGoDown:null,canGoLeft:null,canGoRight:null,canGoUp:null})}[oe](){return super[oe]&&super[oe](),this[ue]()}[ie](){return super[ie]&&super[ie](),this[le]()}[ce](){return super[ce]&&super[ce](),this[Be]&&this[Be].rightToLeft?this[ue]():this[de]()}[he](){return super[he]&&super[he](),this[Be]&&this[Be].rightToLeft?this[de]():this[ue]()}[pe](){return super[pe]&&super[pe](),this[ae]()}[me](){return super[me]&&super[me](),this[de]()}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};if(t.canGoNext||t.canGoPrevious||t.languageDirection||t.orientation||t.rightToLeft){const{canGoNext:t,canGoPrevious:n,orientation:r,rightToLeft:o}=e,i="horizontal"===r||"both"===r,a="vertical"===r||"both"===r,l=a&&t,c=!!i&&(o?t:n),u=!!i&&(o?n:t),d=a&&n;Object.assign(s,{canGoDown:l,canGoLeft:c,canGoRight:u,canGoUp:d})}return s}}}(function(e){return class extends e{get items(){return this[Be]?this[Be].items:null}[Ce](e){if(super[Ce]&&super[Ce](e),!this[se]&&e.items&&this[Ee]){const e=new CustomEvent("items-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("itemschange",{bubbles:!0});this.dispatchEvent(t)}}}}(function(e){return class extends e{[J](e,t={}){const s=void 0!==t.direction?t.direction:1,n=void 0!==t.index?t.index:e.currentIndex,r=void 0!==t.wrap?t.wrap:e.cursorOperationsWrap,{items:o}=e,i=o?o.length:0;if(0===i)return-1;if(r){let t=(n%i+i)%i;const r=((t-s)%i+i)%i;for(;t!==r;){if(!e.availableItemFlags||e.availableItemFlags[t])return t;t=((t+s)%i+i)%i}}else for(let t=n;t>=0&&t<i;t+=s)if(!e.availableItemFlags||e.availableItemFlags[t])return t;return-1}get[X](){return Object.assign(super[X]||{},{currentIndex:-1,desiredCurrentIndex:null,currentItem:null,currentItemRequired:!1,cursorOperationsWrap:!1})}[ae](){return super[ae]&&super[ae](),xs(this,0,1)}[le](){return super[le]&&super[le](),xs(this,this[Be].items.length-1,-1)}[ue](){super[ue]&&super[ue]();const{currentIndex:e,items:t}=this[Be];return xs(this,e<0&&t?0:e+1,1)}[de](){super[de]&&super[de]();const{currentIndex:e,items:t}=this[Be];return xs(this,e<0&&t?t.length-1:e-1,-1)}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};if(t.availableItemFlags||t.items||t.currentIndex||t.currentItemRequired){const{currentIndex:n,desiredCurrentIndex:r,currentItem:o,currentItemRequired:i,items:a}=e,l=a?a.length:0;let c,u=r;if(t.items&&!t.currentIndex&&o&&l>0&&a[n]!==o){const e=a.indexOf(o);e>=0&&(u=e)}else t.currentIndex&&(n<0&&null!==o||n>=0&&(0===l||a[n]!==o)||null===r)&&(u=n);i&&u<0&&(u=0),u<0?(u=-1,c=-1):0===l?c=-1:(c=Math.max(Math.min(l-1,u),0),c=this[J](e,{direction:1,index:c,wrap:!1}),c<0&&(c=this[J](e,{direction:-1,index:c-1,wrap:!1})));const d=a&&a[c]||null;Object.assign(s,{currentIndex:c,desiredCurrentIndex:u,currentItem:d})}return s}}}(function(e){return class extends e{get[X](){return Object.assign(super[X]||{},{texts:null})}[re](e){return super[re]?super[re](e):(t=e).getAttribute("aria-label")||t.getAttribute("alt")||t.innerText||t.textContent||"";var t}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};if(t.items){const{items:t}=e,n=function(e,t){return e?Array.from(e,(e=>t(e))):null}(t,this[re]);n&&(Object.freeze(n),Object.assign(s,{texts:n}))}return s}}}(_t(es(function(e){return class extends e{[ve](e){let t=!1;if("horizontal"!==this.orientation)switch(e.key){case"PageDown":t=this.pageDown();break;case"PageUp":t=this.pageUp()}return t||super[ve]&&super[ve](e)}get orientation(){return super.orientation||this[Be]&&this[Be].orientation||"both"}pageDown(){return super.pageDown&&super.pageDown(),Es(this,!0)}pageUp(){return super.pageUp&&super.pageUp(),Es(this,!1)}get[Oe](){return super[Oe]||ws(this)}}}(function(e){return class extends e{constructor(){super(),Cs(this)}[ge](e){if(super[ge]&&super[ge](e),null==e||0===e.length)return!1;const t=e.toLowerCase(),s=this[Be].texts.findIndex((s=>s.substr(0,e.length).toLowerCase()===t));if(s>=0){const e=this[Be].currentIndex;return this[Ae]({currentIndex:s}),this[Be].currentIndex!==e}return!1}[ve](e){let t;switch(e.key){case"Backspace":!function(e){const t=e,s=t[Is]?t[Is].length:0;s>0&&(t[Is]=t[Is].substr(0,s-1)),e[ge](t[Is]),Ls(e)}(this),t=!0;break;case"Escape":Cs(this);break;default:e.ctrlKey||e.metaKey||e.altKey||1!==e.key.length||function(e,t){const s=e,n=s[Is]||"";s[Is]=n+t,e[ge](s[Is]),Ls(e)}(this,e.key)}return t||super[ve]&&super[ve](e)}}}(ts(function(e){return function(e){return class extends e{get[X](){return Object.assign(super[X]||{},{items:null})}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};if(t.content){const t=e.content,n=t?Array.prototype.filter.call(t,(e=>{return(t=e)instanceof Element&&(!t.localName||Ts.indexOf(t.localName)<0);var t})):null;n&&Object.freeze(n),Object.assign(s,{items:n})}return s}}}(Os(e))}(function(e){return class extends e{constructor(){super(),this.addEventListener("mousedown",(e=>{0===e.button&&(this[Ee]=!0,this[Ke](e),this[Ee]=!1)}))}[Ie](e){super[Ie]&&super[Ie](e),this[se]&&Object.assign(this.style,{touchAction:"manipulation",mozUserSelect:"none",msUserSelect:"none",webkitUserSelect:"none",userSelect:"none"})}[Ke](e){const t=e.composedPath?e.composedPath()[0]:e.target,{items:s,currentItemRequired:n}=this[Be];if(s&&t instanceof Node){const r=E(s,t),o=r>=0?s[r]:null;(o&&!o.disabled||!o&&!n)&&(this[Ae]({currentIndex:r}),e.stopPropagation())}}}}(U))))))))))))))),Ds=class extends As{get[X](){return Object.assign(super[X],{availableItemFlags:null,highlightCurrentItem:!0,orientation:"vertical",currentItemFocused:!1})}async flashCurrentItem(){const e=this[Be].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Ae]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Ae]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[Ie](e){super[Ie](e),this[se]&&(this.addEventListener("disabledchange",(e=>{this[Ee]=!0;const t=e.target,{items:s}=this[Be],n=null===s?-1:s.indexOf(t);if(n>=0){const e=this[Be].availableItemFlags.slice();e[n]=!t.disabled,this[Ae]({availableItemFlags:e})}this[Ee]=!1})),"PointerEvent"in window?this.addEventListener("pointerdown",(e=>this[Ke](e))):this.addEventListener("touchstart",(e=>this[Ke](e))),this.removeAttribute("tabindex"));const{currentIndex:t,items:s}=this[Be];if((e.items||e.currentIndex||e.highlightCurrentItem)&&s){const{highlightCurrentItem:e}=this[Be];s.forEach(((s,n)=>{s.toggleAttribute("current",e&&n===t)}))}(e.items||e.currentIndex||e.currentItemFocused||e.focusVisible)&&s&&s.forEach(((e,s)=>{const n=s===t,r=t<0&&0===s;this[Be].currentItemFocused?n||r||e.removeAttribute("tabindex"):(n||r)&&(e.tabIndex=0)}))}[Ce](e){if(super[Ce](e),!this[se]&&e.currentIndex&&!this[Be].currentItemFocused){const{currentItem:e}=this[Be];(e instanceof HTMLElement?e:this).focus(),this[Ae]({currentItemFocused:!0})}}get[Oe](){return this[fe].content}[Fe](e,t){const s=super[Fe](e,t);if(t.currentIndex&&Object.assign(s,{currentItemFocused:!1}),t.items){const{items:t}=e,n=null===t?null:t.map((e=>!e.disabled));Object.assign(s,{availableItemFlags:n})}return s}get[Ze](){return A.html`
      <style>
        :host {
          box-sizing: border-box;
          cursor: default;
          display: inline-flex;
          -webkit-tap-highlight-color: transparent;
          touch-action: manipulation;
        }

        #content {
          display: flex;
          flex: 1;
          flex-direction: column;
          max-height: 100%;
          overflow-x: hidden;
          overflow-y: auto;
          -webkit-overflow-scrolling: touch; /* for momentum scrolling */
        }
        
        ::slotted(*) {
          flex-shrink: 0;
          outline: none;
          touch-action: manipulation;
        }

        ::slotted(option) {
          font: inherit;
          min-height: inherit;
        }
      </style>
      <div id="content" role="none">
        <slot></slot>
      </div>
    `}},Ms=class extends Ds{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host ::slotted(*) {
            padding: 0.25em 1em;
          }
          
          :host ::slotted([current]) {
            background: highlight;
            color: highlighttext;
          }

          @media (pointer: coarse) {
            ::slotted(*) {
              padding: 1em;
            }
          }
        </style>
      `),e}};customElements.define("elix-menu",class extends Ms{});const Rs=Symbol("documentMouseupListener");async function Bs(e){const t=this,s=t[De].elementsFromPoint(e.clientX,e.clientY);if(t.opened){const e=s.indexOf(t[fe].source)>=0,n=t[fe].popup,r=s.indexOf(n)>=0,o=n.frame&&s.indexOf(n.frame)>=0;e?t[Be].dragSelect&&(t[Ee]=!0,t[Ae]({dragSelect:!1}),t[Ee]=!1):r||o||(t[Ee]=!0,await t.close(),t[Ee]=!1)}}function Fs(e){e[Be].opened&&e.isConnected?e[Rs]||(e[Rs]=Bs.bind(e),document.addEventListener("mouseup",e[Rs])):e[Rs]&&(document.removeEventListener("mouseup",e[Rs]),e[Rs]=null)}function js(e){return class extends e{get[X](){return Object.assign(super[X]||{},{disabled:!1})}get disabled(){return this[Be].disabled}set disabled(e){this[Ae]({disabled:e})}[Ce](e){if(super[Ce]&&super[Ce](e),e.disabled&&(this.toggleAttribute("disabled",this.disabled),this[Ee])){const e=new CustomEvent("disabled-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("disabledchange",{bubbles:!0});this.dispatchEvent(t)}}}}const Hs=Symbol("closePromise"),Ns=Symbol("closeResolve");function zs(e){return class extends e{attributeChangedCallback(e,t,s){if("opened"===e){const t=w(e,s);this.opened!==t&&(this.opened=t)}else super.attributeChangedCallback(e,t,s)}async close(e){super.close&&await super.close(),this[Ae]({closeResult:e}),await this.toggle(!1)}get closed(){return this[Be]&&!this[Be].opened}get closeFinished(){return this[Be].openCloseEffects?"close"===this[Be].effect&&"after"===this[Be].effectPhase:this.closed}get closeResult(){return this[Be].closeResult}get[X](){const e={closeResult:null,opened:!1};return this[Re]&&Object.assign(e,{effect:"close",effectPhase:"after",openCloseEffects:!0}),Object.assign(super[X]||{},e)}async open(){super.open&&await super.open(),this[Ae]({closeResult:void 0}),await this.toggle(!0)}get opened(){return this[Be]&&this[Be].opened}set opened(e){this[Ae]({closeResult:void 0}),this.toggle(e)}[Ce](e){if(super[Ce]&&super[Ce](e),e.opened&&this[Ee]){const e=new CustomEvent("opened-changed",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});this.dispatchEvent(e);const t=new CustomEvent("openedchange",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});if(this.dispatchEvent(t),this[Be].opened){const e=new CustomEvent("opened",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("open",{bubbles:!0});this.dispatchEvent(t)}else{const e=new CustomEvent("closed",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(e);const t=new CustomEvent("close",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(t)}}const t=this[Ns];this.closeFinished&&t&&(this[Ns]=null,this[Hs]=null,t(this[Be].closeResult))}async toggle(e=!this.opened){if(super.toggle&&await super.toggle(e),e!==this[Be].opened){const t={opened:e};this[Be].openCloseEffects&&(t.effect=e?"open":"close","after"===this[Be].effectPhase&&(t.effectPhase="before")),await this[Ae](t)}}whenClosed(){return this[Hs]||(this[Hs]=new Promise((e=>{this[Ns]=e}))),this[Hs]}}}const Ws=function(e){return class extends e{get[X](){return Object.assign(super[X],{role:null})}[Ie](e){if(super[Ie]&&super[Ie](e),e.role){const{role:e}=this[Be];e?this.setAttribute("role",e):this.removeAttribute("role")}}get role(){return super.role}set role(e){super.role=e,this[Le]||this[Ae]({role:e})}}}(U),Vs=class extends Ws{get[X](){return Object.assign(super[X],{role:"none"})}get[Ze](){return A.html`
      <style>
        :host {
          display: inline-block;
          height: 100%;
          left: 0;
          position: fixed;
          top: 0;
          touch-action: manipulation;
          width: 100%;
        }
      </style>
      <slot></slot>
    `}},Ys=class extends U{get[Ze](){return A.html`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }
      </style>
      <slot></slot>
    `}},Us=Symbol("appendedToDocument"),Gs=Symbol("assignedZIndex"),qs=Symbol("restoreFocusToElement");function Ks(e){const t=function(){const e=document.body.querySelectorAll("*"),t=Array.from(e,(e=>{const t=getComputedStyle(e);let s=0;if("static"!==t.position&&"auto"!==t.zIndex){const e=t.zIndex?parseInt(t.zIndex):0;s=isNaN(e)?0:e}return s}));return Math.max(...t)}()+1;e[Gs]=t,e.style.zIndex=t.toString()}function Zs(e){const t=getComputedStyle(e).zIndex,s=e.style.zIndex,n=!isNaN(parseInt(s));if("auto"===t)return n;if("0"===t&&!n){const t=e.assignedSlot||(e instanceof ShadowRoot?e.host:e.parentNode);if(!(t instanceof HTMLElement))return!0;if(!Zs(t))return!1}return!0}const $s=zs(function(e){return class extends e{get autoFocus(){return this[Be].autoFocus}set autoFocus(e){this[Ae]({autoFocus:e})}get[X](){return Object.assign(super[X]||{},{autoFocus:!0,persistent:!1})}async open(){this[Be].persistent||this.isConnected||(this[Us]=!0,document.body.append(this)),super.open&&await super.open()}[Ie](e){if(super[Ie]&&super[Ie](e),this[se]&&this.addEventListener("blur",(e=>{const t=e.relatedTarget||document.activeElement;t instanceof HTMLElement&&(x(this,t)||(this.opened?this[qs]=t:(t.focus(),this[qs]=null)))})),(e.effectPhase||e.opened||e.persistent)&&!this[Be].persistent){const e=void 0===this.closeFinished?this.closed:this.closeFinished;this.style.display=e?"none":"",e?this[Gs]&&(this.style.zIndex="",this[Gs]=null):this[Gs]?this.style.zIndex=this[Gs]:Zs(this)||Ks(this)}}[Ce](e){if(super[Ce]&&super[Ce](e),this[se]&&this[Be].persistent&&!Zs(this)&&Ks(this),e.opened&&this[Be].autoFocus)if(this[Be].opened){this[qs]||document.activeElement===document.body||(this[qs]=document.activeElement);const e=T(this);e&&e.focus()}else this[qs]&&(this[qs].focus(),this[qs]=null);!this[se]&&!this[Be].persistent&&this.closeFinished&&this[Us]&&(this[Us]=!1,this.parentNode&&this.parentNode.removeChild(this))}}}(Os(U)));function Js(e,t,s){if(!s||s.backdropPartType){const{backdropPartType:s}=t,n=e.getElementById("backdrop");n&&Z(n,s)}if(!s||s.framePartType){const{framePartType:s}=t,n=e.getElementById("frame");n&&Z(n,s)}}const Qs=Symbol("implicitCloseListener");async function Xs(e){const t=this,s=e.relatedTarget||document.activeElement;s instanceof Element&&!x(t,s)&&(t[Ee]=!0,await t.close({canceled:"window blur"}),t[Ee]=!1)}async function _s(e){const t=this,s="resize"!==e.type||t[Be].closeOnWindowResize;!I(t,e)&&s&&(t[Ee]=!0,await t.close({canceled:"window "+e.type}),t[Ee]=!1)}const en=es(function(e){return class extends e{constructor(){super(),this.addEventListener("blur",Xs.bind(this))}get closeOnWindowResize(){return this[Be].closeOnWindowResize}set closeOnWindowResize(e){this[Ae]({closeOnWindowResize:e})}get[X](){return Object.assign(super[X]||{},{closeOnWindowResize:!0,role:"alert"})}[ve](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super.keydown&&super.keydown(e)||!1}[Ie](e){if(super[Ie]&&super[Ie](e),e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}[Ce](e){var t;super[Ce]&&super[Ce](e),e.opened&&(this.opened?("requestIdleCallback"in window?window.requestIdleCallback:setTimeout)((()=>{var e;this.opened&&((e=this)[Qs]=_s.bind(e),window.addEventListener("blur",e[Qs]),window.addEventListener("resize",e[Qs]),window.addEventListener("scroll",e[Qs]))})):(t=this)[Qs]&&(window.removeEventListener("blur",t[Qs]),window.removeEventListener("resize",t[Qs]),window.removeEventListener("scroll",t[Qs]),t[Qs]=null))}get role(){return super.role}set role(e){super.role=e,this[Le]||this[Ae]({role:e})}}}(class extends $s{get backdrop(){return this[fe]&&this[fe].backdrop}get backdropPartType(){return this[Be].backdropPartType}set backdropPartType(e){this[Ae]({backdropPartType:e})}get[X](){return Object.assign(super[X],{backdropPartType:Vs,framePartType:Ys})}get frame(){return this[fe].frame}get framePartType(){return this[Be].framePartType}set framePartType(e){this[Ae]({framePartType:e})}[Ie](e){super[Ie](e),Js(this[De],this[Be],e)}[Ce](e){super[Ce](e),e.opened&&this[Be].content&&this[Be].content.forEach((e=>{e[$]&&e[$]()}))}get[Ze](){const e=A.html`
      <style>
        :host {
          align-items: center;
          display: inline-flex;
          flex-direction: column;
          justify-content: center;
          max-height: 100vh;
          max-width: 100vw;
          outline: none;
          position: fixed;
          -webkit-tap-highlight-color: transparent;
        }

        [part~="frame"] {
          box-sizing: border-box;
          display: flex;
          flex-direction: column;
          max-height: 100%;
          max-width: 100%;
          overscroll-behavior: contain;
          pointer-events: initial;
          position: relative;
        }

        #frameContent {
          display: flex;
          flex: 1;
          flex-direction: column;
          height: 100%;
          overflow: hidden;
          width: 100%;
        }
      </style>
      <div id="backdrop" part="backdrop" tabindex="-1"></div>
      <div id="frame" part="frame" role="none">
        <div id="frameContent">
          <slot></slot>
        </div>
      </div>
    `;return Js(e.content,this[Be]),e}}));async function tn(e){const t=this;t[Ee]=!0,await t.close({canceled:"mousedown outside"}),t[Ee]=!1,e.preventDefault(),e.stopPropagation()}const sn=class extends en{[Ie](e){super[Ie](e),e.backdropPartType&&(this[fe].backdrop.addEventListener("mousedown",tn.bind(this)),"PointerEvent"in window||this[fe].backdrop.addEventListener("touchend",tn))}},nn=Symbol("resizeListener"),rn=js(st(ts(zs(U))));function on(e){const t=window.innerHeight,s=window.innerWidth,n=e[fe].popup.getBoundingClientRect(),r=e.getBoundingClientRect(),o=n.height,i=n.width,{horizontalAlign:a,popupPosition:l,rightToLeft:c}=e[Be],u=r.top,d=Math.ceil(t-r.bottom),h=r.right,p=Math.ceil(s-r.left),g=o<=u,m=o<=d,b="below"===l,f=b&&(m||d>=u)||!b&&!g&&d>=u,y=f&&m||!f&&g?null:f?d:u,w=f?"below":"above";let v,x,T;if("stretch"===a)v=0,x=0,T=null;else{const e="left"===a||(c?"end"===a:"start"===a),t=e&&(i<=p||p>=h)||!e&&!(i<=h)&&p>=h;v=t?0:null,x=t?null:0,T=t&&p||!t&&h?null:t?p:h}e[Ae]({calculatedFrameMaxHeight:y,calculatedFrameMaxWidth:T,calculatedPopupLeft:v,calculatedPopupPosition:w,calculatedPopupRight:x,popupMeasured:!0})}function an(e,t,s){if(!s||s.popupPartType){const{popupPartType:s}=t,n=e.getElementById("popup");n&&Z(n,s)}if(!s||s.sourcePartType){const{sourcePartType:s}=t,n=e.getElementById("source");n&&Z(n,s)}}const ln=js(U),cn=class extends ln{get[X](){return Object.assign(super[X],{direction:"down"})}get direction(){return this[Be].direction}set direction(e){this[Ae]({direction:e})}[Ie](e){if(super[Ie](e),e.direction){const{direction:e}=this[Be];this[fe].downIcon.style.display="down"===e?"block":"none",this[fe].upIcon.style.display="up"===e?"block":"none"}}get[Ze](){return A.html`
      <style>
        :host {
          display: inline-block;
        }
      </style>
      <div id="downIcon" part="toggle-icon down-icon">
        <slot name="down-icon"></slot>
      </div>
      <div id="upIcon" part="toggle-icon up-icon">
        <slot name="up-icon"></slot>
      </div>
    `}};function un(e,t,s){if(!s||s.popupTogglePartType){const{popupTogglePartType:s}=t,n=e.getElementById("popupToggle");n&&Z(n,s)}}const dn=rt(es(function(e){return class extends e{connectedCallback(){super.connectedCallback(),Fs(this)}get[X](){return Object.assign(super[X],{dragSelect:!0})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Fs(this)}[Ce](e){super[Ce](e),e.opened&&Fs(this)}[Fe](e,t){const s=super[Fe](e,t);return t.opened&&e.opened&&Object.assign(s,{dragSelect:!0}),s}}}(function(e){return class extends e{get[X](){return Object.assign(super[X],{popupTogglePartType:cn})}get popupTogglePartType(){return this[Be].popupTogglePartType}set popupTogglePartType(e){this[Ae]({popupTogglePartType:e})}[Ie](e){if(super[Ie](e),un(this[De],this[Be],e),e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[Be],t="below"===e?"down":"up",s=this[fe].popupToggle;"direction"in s&&(s.direction=t)}if(e.disabled){const{disabled:e}=this[Be];this[fe].popupToggle.disabled=e}}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="source"]');return t&&t.append(O`
        <div
          id="popupToggle"
          part="popup-toggle"
          exportparts="toggle-icon, down-icon, up-icon"
          tabindex="-1"
        >
          <slot name="toggle-icon"></slot>
        </div>
      `),un(e.content,this[Be]),e.content.append(O`
      <style>
        [part~="popup-toggle"] {
          outline: none;
        }

        [part~="source"] {
          align-items: center;
          display: flex;
        }
      </style>
    `),e}}}(class extends rn{get[X](){return Object.assign(super[X],{ariaHasPopup:"true",horizontalAlign:"start",popupHeight:null,popupMeasured:!1,popupPosition:"below",popupPartType:sn,popupWidth:null,roomAbove:null,roomBelow:null,roomLeft:null,roomRight:null,sourcePartType:"div"})}get[ye](){return this[fe].source}get frame(){return this[fe].popup.frame}get horizontalAlign(){return this[Be].horizontalAlign}set horizontalAlign(e){this[Ae]({horizontalAlign:e})}[Ie](e){if(super[Ie](e),an(this[De],this[Be],e),this[se]||e.ariaHasPopup){const{ariaHasPopup:e}=this[Be];null===e?this[ye].removeAttribute("aria-haspopup"):this[ye].setAttribute("aria-haspopup",this[Be].ariaHasPopup)}if(e.popupPartType&&(this[fe].popup.addEventListener("open",(()=>{this.opened||(this[Ee]=!0,this.open(),this[Ee]=!1)})),this[fe].popup.addEventListener("close",(e=>{if(!this.closed){this[Ee]=!0;const t=e.detail.closeResult;this.close(t),this[Ee]=!1}}))),e.horizontalAlign||e.popupMeasured||e.rightToLeft){const{calculatedFrameMaxHeight:e,calculatedFrameMaxWidth:t,calculatedPopupLeft:s,calculatedPopupPosition:n,calculatedPopupRight:r,popupMeasured:o}=this[Be],i="below"===n,a=i?null:0,l=o?null:0,c=o?"absolute":"fixed",u=s,d=r,h=this[fe].popup;Object.assign(h.style,{bottom:a,left:u,opacity:l,position:c,right:d});const p=h.frame;Object.assign(p.style,{maxHeight:e?e+"px":null,maxWidth:t?t+"px":null}),this[fe].popupContainer.style.top=i?"":"0"}if(e.opened){const{opened:e}=this[Be];this[fe].popup.opened=e}if(e.disabled&&"disabled"in this[fe].source){const{disabled:e}=this[Be];this[fe].source.disabled=e}}[Ce](e){var t;super[Ce](e),e.opened?this.opened?(t=this,setTimeout((()=>{t.opened&&(on(t),function(e){const t=e;t[nn]=()=>{on(e)},window.addEventListener("resize",t[nn])}(t))}))):function(e){const t=e;t[nn]&&(window.removeEventListener("resize",t[nn]),t[nn]=null)}(this):this.opened&&!this[Be].popupMeasured&&on(this)}get popupPosition(){return this[Be].popupPosition}set popupPosition(e){this[Ae]({popupPosition:e})}get popupPartType(){return this[Be].popupPartType}set popupPartType(e){this[Ae]({popupPartType:e})}get sourcePartType(){return this[Be].sourcePartType}set sourcePartType(e){this[Ae]({sourcePartType:e})}[Fe](e,t){const s=super[Fe](e,t);return t.opened&&!e.opened&&Object.assign(s,{calculatedFrameMaxHeight:null,calculatedFrameMaxWidth:null,calculatedPopupLeft:null,calculatedPopupPosition:null,calculatedPopupRight:null,popupMeasured:!1}),s}get[Ze](){const e=super[Ze];return e.content.append(O`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }

        [part~="source"] {
          height: 100%;
          -webkit-tap-highlight-color: transparent;
          touch-action: manipulation;
          width: 100%;
        }

        #popupContainer {
          height: 0;
          outline: none;
          position: absolute;
          width: 100%;
        }

        [part~="popup"] {
          align-items: initial;
          height: initial;
          justify-content: initial;
          left: initial;
          outline: none;
          position: absolute;
          top: initial;
          width: initial;
        }
      </style>
      <div id="source" part="source">
        <slot name="source"></slot>
      </div>
      <div id="popupContainer" role="none">
        <div id="popup" part="popup" exportparts="backdrop, frame" role="none">
          <slot></slot>
        </div>
      </div>
    `),an(e.content,this[Be]),e}})))),hn=class extends dn{get[X](){return Object.assign(super[X],{sourcePartType:"button"})}[ve](e){let t;switch(e.key){case" ":case"ArrowDown":case"ArrowUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0)}if(t=super[ve]&&super[ve](e),!t&&this.opened&&!e.metaKey&&!e.altKey)switch(e.key){case"ArrowDown":case"ArrowLeft":case"ArrowRight":case"ArrowUp":case"End":case"Home":case"PageDown":case"PageUp":case" ":t=!0}return t}[Ie](e){if(super[Ie](e),this[se]&&this[fe].source.addEventListener("focus",(async e=>{const t=I(this[fe].popup,e),s=null!==this[Be].popupHeight;!t&&this.opened&&s&&(this[Ee]=!0,await this.close(),this[Ee]=!1)})),e.opened){const{opened:e}=this[Be];this.toggleAttribute("opened",e),this[fe].source.setAttribute("aria-expanded",e.toString())}e.sourcePartType&&this[fe].source.addEventListener("mousedown",(e=>{if(this.disabled)return void e.preventDefault();const t=e;t.button&&0!==t.button||(setTimeout((()=>{this.opened||(this[Ee]=!0,this.open(),this[Ee]=!1)})),e.stopPropagation())})),e.popupPartType&&this[fe].popup.removeAttribute("tabindex")}get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          [part~="source"] {
            cursor: default;
            outline: none;
            -webkit-tap-highlight-color: transparent;
            touch-action: manipulation;
            -moz-user-select: none;
            -ms-user-select: none;
            -webkit-user-select: none;
            user-select: none;
          }

          :host([opened][focus-visible]) {
            outline: none;
          }
        </style>
      `),e}},pn=Symbol("documentMousemoveListener");function gn(e){const t=this,{hasHoveredOverItemSinceOpened:s,opened:n}=t[Be];if(n){const n=e.composedPath?e.composedPath()[0]:e.target;if(n&&n instanceof Node){const e=t.items,r=E(e,n),o=e[r],i=o&&!o.disabled?r:-1;(s||i>=0)&&i!==t[Be].currentIndex&&(t[Ee]=!0,t[Ae]({currentIndex:i}),i>=0&&!s&&t[Ae]({hasHoveredOverItemSinceOpened:!0}),t[Ee]=!1)}}}function mn(e){e[Be].opened&&e.isConnected?e[pn]||(e[pn]=gn.bind(e),document.addEventListener("mousemove",e[pn])):e[pn]&&(document.removeEventListener("mousemove",e[pn]),e[pn]=null)}async function bn(e){const t=e[Ee],s=e[Be].currentIndex>=0,n=s?e.items[e[Be].currentIndex]:void 0,r=e[Be].popupList;s&&"flashCurrentItem"in r&&await r.flashCurrentItem();const o=e[Ee];e[Ee]=t,await e.close(n),e[Ee]=o}const fn=function(e){return class extends e{connectedCallback(){super.connectedCallback(),mn(this)}get[X](){return Object.assign(super[X],{currentIndex:-1,hasHoveredOverItemSinceOpened:!1,popupList:null})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),mn(this)}[ve](e){let t=!1;switch(e.key){case"Enter":this.opened&&(bn(this),t=!0)}return t||super[ve]&&super[ve](e)||!1}[Ie](e){if(super[Ie]&&super[Ie](e),e.popupList){const{popupList:e}=this[Be];e&&(e.addEventListener("mouseup",(async e=>{const t=this[Be].currentIndex;this[Be].dragSelect||t>=0?(e.stopPropagation(),this[Ee]=!0,await bn(this),this[Ee]=!1):e.stopPropagation()})),e.addEventListener("currentindexchange",(e=>{this[Ee]=!0;const t=e;this[Ae]({currentIndex:t.detail.currentIndex}),this[Ee]=!1})))}if(e.currentIndex||e.popupList){const{currentIndex:e,popupList:t}=this[Be];t&&"currentIndex"in t&&(t.currentIndex=e)}}[Ce](e){if(super[Ce]&&super[Ce](e),e.opened){if(this[Be].opened){const{popupList:e}=this[Be];e.scrollCurrentItemIntoView&&setTimeout((()=>{e.scrollCurrentItemIntoView()}))}mn(this)}}[Fe](e,t){const s=super[Fe]?super[Fe](e,t):{};return t.opened&&e.opened&&Object.assign(s,{hasHoveredOverItemSinceOpened:!1}),s}}}(hn);function yn(e,t,s){if(!s||s.menuPartType){const{menuPartType:s}=t,n=e.getElementById("menu");n&&Z(n,s)}}const wn=class extends fn{get[X](){return Object.assign(super[X],{menuPartType:Ds})}get items(){const e=this[fe]&&this[fe].menu;return e?e.items:null}get menuPartType(){return this[Be].menuPartType}set menuPartType(e){this[Ae]({menuPartType:e})}[Ie](e){super[Ie](e),yn(this[De],this[Be],e),e.menuPartType&&(this[fe].menu.addEventListener("blur",(async e=>{const t=e.relatedTarget||document.activeElement;this.opened&&!x(this[fe].menu,t)&&(this[Ee]=!0,await this.close(),this[Ee]=!1)})),this[fe].menu.addEventListener("mousedown",(e=>{0===e.button&&this.opened&&(e.stopPropagation(),e.preventDefault())})))}[Ce](e){super[Ce](e),e.menuPartType&&this[Ae]({popupList:this[fe].menu})}[Fe](e,t){const s=super[Fe](e,t);return t.opened&&!e.opened&&Object.assign(s,{currentIndex:-1}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(O`
        <div id="menu" part="menu">
          <slot></slot>
        </div>
      `),yn(e.content,this[Be]),e.content.append(O`
      <style>
        [part~="menu"] {
          max-height: 100%;
        }
      </style>
    `),e}},vn=class extends cs{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          [part~="inner"] {
            background: #eee;
            border: 1px solid #ccc;
            padding: 0.25em 0.5em;
          }
        </style>
      `),e}},xn=class extends cn{get[Ze](){const e=super[Ze],t=e.content.getElementById("downIcon"),s=O`
      <svg
        id="downIcon"
        part="toggle-icon down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 0 l5 5 5 -5 z" />
      </svg>
    `.firstElementChild;t&&s&&K(t,s);const n=e.content.getElementById("upIcon"),r=O`
      <svg
        id="upIcon"
        part="toggle-icon up-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 5 l5 -5 5 5 z" />
      </svg>
    `.firstElementChild;return n&&r&&K(n,r),e.content.append(O`
        <style>
          :host {
            align-items: center;
            display: inline-flex;
            padding: 2px;
          }

          :host(:not([disabled])):hover {
            background: #eee;
          }

          [part~="toggle-icon"] {
            fill: currentColor;
            height: 10px;
            margin: 0.25em;
            width: 10px;
          }
        </style>
      `),e}},Tn=class extends Vs{},Pn=class extends Ys{get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          :host {
            background: white;
            border: 1px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 0px 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
          }
        </style>
      `),e}},En=class extends sn{get[X](){return Object.assign(super[X],{backdropPartType:Tn,framePartType:Pn})}},In=class extends wn{get[X](){return Object.assign(super[X],{menuPartType:Ms,popupPartType:En,popupTogglePartType:xn,sourcePartType:vn})}get[Ze](){const e=super[Ze];return e.content.append(O`
        <style>
          [part~="menu"] {
            background: window;
            border: none;
            padding: 0.5em 0;
          }
        </style>
      `),e}};customElements.define("elix-menu-button",class extends In{});class kn extends(function(e){return class extends e{constructor(){super();!this[Pe]&&this.attachInternals&&(this[Pe]=this.attachInternals())}attributeChangedCallback(e,t,s){if("current"===e){const t=w(e,s);this.current!==t&&(this.current=t)}else super.attributeChangedCallback(e,t,s)}get[X](){return Object.assign(super[X]||{},{current:!1})}[Ie](e){if(super[Ie](e),e.current){const{current:e}=this[Be];S(this,"current",e)}}[Ce](e){if(super[Ce]&&super[Ce](e),e.current){const{current:e}=this[Be],t=new CustomEvent("current-changed",{bubbles:!0,detail:{current:e}});this.dispatchEvent(t);const s=new CustomEvent("currentchange",{bubbles:!0,detail:{current:e}});this.dispatchEvent(s)}}get current(){return this[Be].current}set current(e){this[Ae]({current:e})}}}(js(zt(U)))){}const Sn=kn,Cn=class extends Sn{get[Ze](){return A.html`
      <style>
        :host {
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
          font-size: 10pt;
          white-space: nowrap;
        }

        :host([disabled]) {
          opacity: 0.5;
        }

        #checkmark {
          height: 1em;
          visibility: hidden;
          width: 1em;
        }

        :host([selected]) #checkmark {
          visibility: visible;
        }
      </style>
      <svg id="checkmark" xmlns="http://www.w3.org/2000/svg" viewBox="4 6 18 12">
        <path d="M0 0h24v24H0V0z" fill="none"/>
        <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41L9 16.17z"/>
      </svg>
      <slot></slot>
    `}};customElements.define("elix-menu-item",class extends Cn{});const Ln=class extends U{get disabled(){return!0}[Ie](e){super[Ie](e),this[se]&&this.setAttribute("aria-hidden","true")}},On=class extends Ln{get[Ze](){return A.html`
      <style>
        :host {
          padding: 0 !important;
        }

        hr {
          border-bottom-width: 0px;
          border-color: #fff; /* Ends up as light gray */
          border-top-width: 1px;
          margin: 0.25em 0;
        }
      </style>
      <hr>
    `}};customElements.define("elix-menu-separator",class extends On{});const An=class extends hn{get[X](){return Object.assign(super[X],{popupPartType:En,sourcePartType:vn})}};customElements.define("elix-popup-button",class extends An{}),customElements.define("elix-popup",class extends En{})})();