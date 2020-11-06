(()=>{"use strict";const e=Symbol("defaultState"),t=Symbol("delegatesFocus"),s=Symbol("firstRender"),n=Symbol("focusTarget"),r=Symbol("hasDynamicTemplate"),o=Symbol("ids"),i=Symbol("nativeInternals"),a=Symbol("raiseChangeEvents"),l=Symbol("render"),c=Symbol("renderChanges"),u=Symbol("rendered"),d=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),m=Symbol("shadowRootMode"),g=Symbol("state"),b=Symbol("stateEffects"),f=Symbol("template"),y=Symbol("mousedownListener");function w(e,t){return"boolean"==typeof t?t:"string"==typeof t&&(""===t||e.toLowerCase()===t.toLowerCase())}function v(e){for(const t of k(e)){const e=t[n]||t,s=e;if(e instanceof HTMLElement&&e.tabIndex>=0&&!s.disabled&&!(e instanceof HTMLSlotElement))return e}return null}function x(e,t){let s=t;for(;s;){const t=s.assignedSlot||s.parentNode||s.host;if(t===e)return!0;s=t}return!1}function T(e){const t=O(e,(e=>e instanceof HTMLElement&&e.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&e.tabIndex>=0)),{value:s}=t.next();return s instanceof HTMLElement?s:null}function P(e,t){e[y]&&e.removeEventListener("mousedown",e[y]),t&&(e[y]=e=>{if(0!==e.button)return;const s=v(t[n]||t);s&&(s.focus(),e.preventDefault())},e.addEventListener("mousedown",e[y]))}function I(e,t){return Array.prototype.findIndex.call(e,(e=>e===t||x(e,t)))}function E(e,t){const s=t.composedPath()[0];return e===s||x(e,s)}function*k(e){e&&(yield e,yield*function*(e){let t=e;for(;t=t instanceof HTMLElement&&t.assignedSlot?t.assignedSlot:t instanceof ShadowRoot?t.host:t.parentNode,t;)yield t}(e))}function C(e,t,s){e.toggleAttribute(t,s),e[i]&&e[i].states&&e[i].states.toggle(t,s)}const S={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function L(e,t){const s=[...t],n=e.childNodes.length,r=s.length,o=Math.max(n,r);for(let t=0;t<o;t++){const o=e.childNodes[t],i=s[t];t>=n?e.append(i):t>=r?e.removeChild(e.childNodes[r]):o!==i&&(s.indexOf(o,t)>=t?e.insertBefore(i,o):e.replaceChild(i,o))}}function*O(e,t){let s;if(t(e)&&(yield e),e instanceof HTMLElement&&e.shadowRoot)s=e.shadowRoot.children;else{const t=e instanceof HTMLSlotElement?e.assignedNodes({flatten:!0}):[];s=t.length>0?t:e.childNodes}if(s)for(let e=0;e<s.length;e++)yield*O(s[e],t)}const A=(e,...t)=>D.html(e,...t).content,D={html(e,...t){const s=document.createElement("template");return s.innerHTML=String.raw(e,...t),s}},M={tabindex:"tabIndex"},R={tabIndex:"tabindex"};function j(e){if(e===HTMLElement)return[];const t=Object.getPrototypeOf(e.prototype).constructor;let s=t.observedAttributes;s||(s=j(t));const n=Object.getOwnPropertyNames(e.prototype).filter((t=>{const s=Object.getOwnPropertyDescriptor(e.prototype,t);return s&&"function"==typeof s.set})).map((e=>function(e){let t=R[e];if(!t){const s=/([A-Z])/g;t=e.replace(s,"-$1").toLowerCase(),R[e]=t}return t}(e))).filter((e=>s.indexOf(e)<0));return s.concat(n)}const B=Symbol("state"),F=Symbol("raiseChangeEventsInNextRender"),N=Symbol("changedSinceLastRender");function H(e,t){const s={};for(const o in t)n=t[o],r=e[o],(n instanceof Date&&r instanceof Date?n.getTime()===r.getTime():n===r)||(s[o]=!0);var n,r;return s}const z=new Map,W=Symbol("shadowIdProxy"),V=Symbol("proxyElement"),Y={get(e,t){const s=e[V][p];return s&&"string"==typeof t?s.getElementById(t):null}};function U(e){let t=e[r]?void 0:z.get(e.constructor);if(void 0===t&&(t=e[f],t)){if(!(t instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${e.constructor.name} must return an HTMLTemplateElement.`;e[r]||z.set(e.constructor,t)}return t}const G=function(e){return class extends e{attributeChangedCallback(e,t,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(e,t,s),s!==t&&!this[d]){const t=function(e){let t=M[e];if(!t){const s=/-([a-z])/g;t=e.replace(s,(e=>e[1].toUpperCase())),M[e]=t}return t}(e);if(t in this){const n=S[e]?w(e,s):s;this[t]=n}}}static get observedAttributes(){return j(this)}}}(function(t){class n extends t{constructor(){super(),this[s]=void 0,this[a]=!1,this[N]=null,this[h](this[e])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[c]()}get[e](){return super[e]||{}}[l](e){super[l]&&super[l](e)}[c](){void 0===this[s]&&(this[s]=!0);const e=this[N];if(this[s]||e){const t=this[a];this[a]=this[F],this[d]=!0,this[l](e),this[d]=!1,this[N]=null,this[u](e),this[s]=!1,this[a]=t,this[F]=t}}[u](e){super[u]&&super[u](e)}async[h](e){this[d]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:t,changed:n}=function(e,t){const s=Object.assign({},e[B]),n={};let r=t;for(;;){const t=H(s,r);if(0===Object.keys(t).length)break;Object.assign(s,r),Object.assign(n,t),r=e[b](s,t)}return{state:s,changed:n}}(this,e);if(this[B]&&0===Object.keys(n).length)return;Object.freeze(t),this[B]=t,this[a]&&(this[F]=!0);const r=void 0===this[s]||null!==this[N];this[N]=Object.assign(this[N]||{},n),this.isConnected&&!r&&(await Promise.resolve(),this[c]())}get[g](){return this[B]}[b](e,t){return super[b]?super[b](e,t):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(n.prototype,"state",{get(){return this[g]}}),n}(function(e){return class extends e{get[o](){if(!this[W]){const e={[V]:this};this[W]=new Proxy(e,Y)}return this[W]}[l](e){if(super[l]&&super[l](e),void 0===this[s]||this[s]){const e=U(this);if(e){const s=this.attachShadow({delegatesFocus:this[t],mode:this[m]}),n=document.importNode(e.content,!0);s.append(n),this[p]=s}}}get[m](){return"open"}}}(HTMLElement))),q=new Map;function K(e){if("function"==typeof e){let t;try{t=new e}catch(s){if("TypeError"!==s.name)throw s;!function(e){let t;const s=e.name&&e.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const e=/([A-Z])/g;t=s[0].replace(e,((e,t,s)=>s>0?"-"+t:t)).toLowerCase()}else t="custom-element";let n,r=q.get(t)||0;for(;n=`${t}-${r}`,customElements.get(n);r++);customElements.define(n,e),q.set(t,r+1)}(e),t=new e}return t}return document.createElement(e)}function Z(e,t){const s=e.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";return(e instanceof HTMLElement||e instanceof SVGElement)&&(t instanceof HTMLElement||t instanceof SVGElement)&&(Array.prototype.forEach.call(e.attributes,(e=>{t.getAttribute(e.name)||"class"===e.name||"style"===e.name||t.setAttribute(e.name,e.value)})),Array.prototype.forEach.call(e.classList,(e=>{t.classList.add(e)})),Array.prototype.forEach.call(e.style,(s=>{t.style[s]||(t.style[s]=e.style[s])}))),t.append(...e.childNodes),s.replaceChild(t,e),t}function $(e,t){if("function"==typeof t&&e.constructor===t||"string"==typeof t&&e instanceof Element&&e.localName===t)return e;{const s=K(t);return Z(e,s),s}}const J=Symbol("applyElementData"),Q=Symbol("checkSize"),X=Symbol("closestAvailableItemIndex"),_=Symbol("contentSlot"),ee=e,te=Symbol("defaultTabIndex"),se=t,ne=Symbol("effectEndTarget"),re=s,oe=n,ie=Symbol("getItemText"),ae=Symbol("goDown"),le=Symbol("goEnd"),ce=Symbol("goFirst"),ue=Symbol("goLast"),de=Symbol("goLeft"),he=Symbol("goNext"),pe=Symbol("goPrevious"),me=Symbol("goRight"),ge=Symbol("goStart"),be=Symbol("goToItemWithPrefix"),fe=Symbol("goUp"),ye=r,we=o,ve=Symbol("inputDelegate"),xe=Symbol("itemsDelegate"),Te=Symbol("keydown"),Pe=(Symbol("matchText"),Symbol("mouseenter")),Ie=Symbol("mouseleave"),Ee=i,ke=a,Ce=l,Se=c,Le=Symbol("renderDataToElement"),Oe=u,Ae=d,De=Symbol("scrollTarget"),Me=h,Re=p,je=m,Be=Symbol("startEffect"),Fe=g,Ne=b,He=Symbol("swipeDown"),ze=Symbol("swipeDownComplete"),We=Symbol("swipeLeft"),Ve=Symbol("swipeLeftTransitionEnd"),Ye=Symbol("swipeRight"),Ue=Symbol("swipeRightTransitionEnd"),Ge=Symbol("swipeUp"),qe=Symbol("swipeUpComplete"),Ke=Symbol("swipeStart"),Ze=Symbol("swipeTarget"),$e=Symbol("tap"),Je=f,Qe=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:Q,closestAvailableItemIndex:X,contentSlot:_,defaultState:ee,defaultTabIndex:te,delegatesFocus:se,effectEndTarget:ne,firstRender:re,focusTarget:oe,getItemText:ie,goDown:ae,goEnd:le,goFirst:ce,goLast:ue,goLeft:de,goNext:he,goPrevious:pe,goRight:me,goStart:ge,goToItemWithPrefix:be,goUp:fe,hasDynamicTemplate:ye,ids:we,inputDelegate:ve,itemsDelegate:xe,keydown:Te,mouseenter:Pe,mouseleave:Ie,nativeInternals:Ee,event,raiseChangeEvents:ke,render:Ce,renderChanges:Se,renderDataToElement:Le,rendered:Oe,rendering:Ae,scrollTarget:De,setState:Me,shadowRoot:Re,shadowRootMode:je,startEffect:Be,state:Fe,stateEffects:Ne,swipeDown:He,swipeDownComplete:ze,swipeLeft:We,swipeLeftTransitionEnd:Ve,swipeRight:Ye,swipeRightTransitionEnd:Ue,swipeUp:Ge,swipeUpComplete:qe,swipeStart:Ke,swipeTarget:Ze,tap:$e,template:Je,toggleSelectedFlag:Qe}});const Xe=document.createElement("div");Xe.attachShadow({mode:"open",delegatesFocus:!0});const _e=Xe.shadowRoot.delegatesFocus;function et(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{composeFocus:!_e})}[Ce](e){super[Ce]&&super[Ce](e),this[re]&&this.addEventListener("mousedown",(e=>{if(this[Fe].composeFocus&&0===e.button&&e.target instanceof Element){const t=v(e.target);t&&(t.focus(),e.preventDefault())}}))}}}function tt(e){return class extends e{get ariaLabel(){return this[Fe].ariaLabel}set ariaLabel(e){this[Fe].removingAriaAttribute||this[Me]({ariaLabel:e})}get ariaLabelledby(){return this[Fe].ariaLabelledby}set ariaLabelledby(e){this[Fe].removingAriaAttribute||this[Me]({ariaLabelledby:e})}get[ee](){return Object.assign(super[ee]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[Ce](e){if(super[Ce]&&super[Ce](e),this[re]&&this.addEventListener("focus",(()=>{this[ke]=!0;const e=nt(this,this[Fe]);this[Me]({inputLabel:e}),this[ke]=!1})),e.inputLabel){const{inputLabel:e}=this[Fe];e?this[ve].setAttribute("aria-label",e):this[ve].removeAttribute("aria-label")}}[Oe](e){super[Oe]&&super[Oe](e),this[re]&&(window.requestIdleCallback||setTimeout)((()=>{const e=nt(this,this[Fe]);this[Me]({inputLabel:e})}));const{ariaLabel:t,ariaLabelledby:s}=this[Fe];e.ariaLabel&&!this[Fe].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",t),this[Me]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),e.ariaLabelledby&&!this[Fe].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Me]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),e.removingAriaAttribute&&this[Fe].removingAriaAttribute&&this[Me]({removingAriaAttribute:!1})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.ariaLabel&&e.ariaLabel||t.selectedText&&e.ariaLabelledby&&this.matches(":focus-within")){const t=nt(this,e);Object.assign(s,{inputLabel:t})}return s}}}function st(e){if("selectedText"in e)return e.selectedText;if("value"in e&&"options"in e){const t=e.value,s=e.options.find((e=>e.value===t));return s?s.innerText:""}return"value"in e?e.value:e.innerText}function nt(e,t){const{ariaLabel:s,ariaLabelledby:n}=t,r=e.isConnected?e.getRootNode():null;let o=null;if(n&&r)o=n.split(" ").map((s=>{const n=r.getElementById(s);return n?n===e&&null!==t.value?t.selectedText:st(n):""})).join(" ");else if(s)o=s;else if(r){const t=e.id;if(t){const e=r.querySelector(`[for="${t}"]`);e instanceof HTMLElement&&(o=st(e))}if(null===o){const t=e.closest("label");t&&(o=st(t))}}return o&&(o=o.trim()),o}let rt=!1;const ot=Symbol("focusVisibleChangedListener");function it(e){return class extends e{constructor(){super(),this.addEventListener("focusout",(e=>{Promise.resolve().then((()=>{const t=e.relatedTarget||document.activeElement,s=this===t,n=x(this,t);!s&&!n&&(this[Me]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[ot]),this[ot]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[Fe].focusVisible!==rt&&this[Me]({focusVisible:rt}),this[ot]||(this[ot]=()=>{this[Me]({focusVisible:rt})},document.addEventListener("focusvisiblechange",this[ot]))}))}))}get[ee](){return Object.assign(super[ee]||{},{focusVisible:!1})}[Ce](e){if(super[Ce]&&super[Ce](e),e.focusVisible){const{focusVisible:e}=this[Fe];this.toggleAttribute("focus-visible",e)}}get[Je](){const e=super[Je]||D.html``;return e.content.append(A`
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
      `),e}}}function at(e){if(rt!==e){rt=e;const t=new CustomEvent("focus-visible-changed",{detail:{focusVisible:rt}});document.dispatchEvent(t);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:rt}});document.dispatchEvent(s)}}function lt(e){return class extends e{get[se](){return!0}focus(e){const t=this[oe];t&&t.focus(e)}get[oe](){return T(this[Re])}}}window.addEventListener("keydown",(()=>{at(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{at(!1)}),{capture:!0});const ct=Symbol("extends"),ut=Symbol("delegatedPropertySetters"),dt={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},ht={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},pt=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],mt={abort:!0,change:!0,reset:!0},gt=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],bt=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],ft=lt(G);class yt extends ft{constructor(){super();!this[Ee]&&this.attachInternals&&(this[Ee]=this.attachInternals())}attributeChangedCallback(e,t,s){if(bt.indexOf(e)>=0){const t=Object.assign({},this[Fe].innerAttributes,{[e]:s});this[Me]({innerAttributes:t})}else super.attributeChangedCallback(e,t,s)}blur(){this.inner.blur()}get[ee](){return Object.assign(super[ee],{innerAttributes:{}})}get[te](){return dt[this.extends]?0:-1}get extends(){return this.constructor[ct]}get inner(){const e=this[we]&&this[we].inner;return e||console.warn("Attempted to get an inner standard element before it was instantiated."),e}getInnerProperty(e){return this[Fe][e]||this[Re]&&this.inner[e]}static get observedAttributes(){return[...super.observedAttributes,...bt]}[Ce](e){super[Ce](e);const t=this.inner;if(this[re]&&((ht[this.extends]||[]).forEach((e=>{t.addEventListener(e,(()=>{const t=new Event(e,{bubbles:mt[e]||!1});this.dispatchEvent(t)}))})),"disabled"in t&&pt.forEach((e=>{this.addEventListener(e,(e=>{t.disabled&&e.stopImmediatePropagation()}))}))),e.tabIndex&&(t.tabIndex=this[Fe].tabIndex),e.innerAttributes){const{innerAttributes:e}=this[Fe];for(const s in e)wt(t,s,e[s])}this.constructor[ut].forEach((s=>{if(e[s]){const e=this[Fe][s];("selectionEnd"===s||"selectionStart"===s)&&null===e||(t[s]=e)}}))}[Oe](e){if(super[Oe](e),e.disabled){const{disabled:e}=this[Fe];void 0!==e&&C(this,"disabled",e)}}setInnerProperty(e,t){this[Fe][e]!==t&&this[Me]({[e]:t})}get[Je](){const e=gt.includes(this.extends)?"block":"inline-block";return D.html`
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
    `}static wrap(e){class t extends yt{}t[ct]=e;const s=document.createElement(e);return function(e,t){const s=Object.getOwnPropertyNames(t);e[ut]=[],s.forEach((s=>{const n=Object.getOwnPropertyDescriptor(t,s);if(!n)return;const r=function(e,t){if("function"==typeof t.value){if("constructor"!==e)return function(e,t){return{configurable:t.configurable,enumerable:t.enumerable,value:function(...t){this.inner[e](...t)},writable:t.writable}}(e,t)}else if("function"==typeof t.get||"function"==typeof t.set)return function(e,t){const s={configurable:t.configurable,enumerable:t.enumerable};return t.get&&(s.get=function(){return this.getInnerProperty(e)}),t.set&&(s.set=function(t){this.setInnerProperty(e,t)}),t.writable&&(s.writable=t.writable),s}(e,t);return null}(s,n);r&&(Object.defineProperty(e.prototype,s,r),r.set&&e[ut].push(s))}))}(t,Object.getPrototypeOf(s)),t}}function wt(e,t,s){S[t]?"string"==typeof s?e.setAttribute(t,""):null===s&&e.removeAttribute(t):null!=s?e.setAttribute(t,s.toString()):e.removeAttribute(t)}const vt=et(tt(it(yt.wrap("button")))),xt=class extends vt{get[ee](){return Object.assign(super[ee],{role:"button"})}get[ve](){return this[we].inner}[$e](){const e=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(e)}get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},Tt=Symbol("wrap");function Pt(e){return class extends e{get arrowButtonOverlap(){return this[Fe].arrowButtonOverlap}set arrowButtonOverlap(e){this[Me]({arrowButtonOverlap:e})}get arrowButtonPartType(){return this[Fe].arrowButtonPartType}set arrowButtonPartType(e){this[Me]({arrowButtonPartType:e})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[pe]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[he]()}attributeChangedCallback(e,t,s){"arrow-button-overlap"===e?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===e?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{arrowButtonOverlap:!0,arrowButtonPartType:xt,orientation:"horizontal",showArrowButtons:!0})}[Ce](e){if(e.arrowButtonPartType){const e=this[we].arrowButtonPrevious;e instanceof HTMLElement&&P(e,null);const t=this[we].arrowButtonNext;t instanceof HTMLElement&&P(t,null)}if(super[Ce]&&super[Ce](e),Et(this[Re],this[Fe],e),e.arrowButtonPartType){const e=this,t=this[we].arrowButtonPrevious;t instanceof HTMLElement&&P(t,e);const s=It(this,(()=>this.arrowButtonPrevious()));t.addEventListener("mousedown",s);const n=this[we].arrowButtonNext;n instanceof HTMLElement&&P(n,e);const r=It(this,(()=>this.arrowButtonNext()));n.addEventListener("mousedown",r)}const{arrowButtonOverlap:t,canGoNext:s,canGoPrevious:n,orientation:r,rightToLeft:o}=this[Fe],i="vertical"===r,a=this[we].arrowButtonPrevious,l=this[we].arrowButtonNext;if(e.arrowButtonOverlap||e.orientation||e.rightToLeft){this[we].arrowDirection.style.flexDirection=i?"column":"row";const e={bottom:null,left:null,right:null,top:null};let s,n;t?Object.assign(e,{position:"absolute","z-index":1}):Object.assign(e,{position:null,"z-index":null}),t&&(i?(Object.assign(e,{left:0,right:0}),s={top:0},n={bottom:0}):(Object.assign(e,{bottom:0,top:0}),o?(s={right:0},n={left:0}):(s={left:0},n={right:0}))),Object.assign(a.style,e,s),Object.assign(l.style,e,n)}if(e.canGoNext&&null!==s&&(l.disabled=!s),e.canGoPrevious&&null!==n&&(a.disabled=!n),e.showArrowButtons){const e=this[Fe].showArrowButtons?null:"none";a.style.display=e,l.style.display=e}}get showArrowButtons(){return this[Fe].showArrowButtons}set showArrowButtons(e){this[Me]({showArrowButtons:e})}[Tt](e){const t=A`
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
      `;Et(t,this[Fe]);const s=t.getElementById("arrowDirectionContainer");s&&(e.replaceWith(t),s.append(e))}}}function It(e,t){return async function(s){0===s.button&&(e[ke]=!0,t()&&s.stopPropagation(),await Promise.resolve(),e[ke]=!1)}}function Et(e,t,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=t,n=e.getElementById("arrowButtonPrevious");n&&$(n,s);const r=e.getElementById("arrowButtonNext");r&&$(r,s)}}Pt.wrap=Tt;const kt=Pt,Ct={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},St=864e5;function Lt(e,t){const s=e.includes("-ca-")?"":"-ca-gregory",n=e.includes("-nu-")?"":"-nu-latn",r=`${e}${s||n?"-u":""}${s}${n}`;return new Intl.DateTimeFormat(r,t)}function Ot(e,t){return null===e&&null===t||null!==e&&null!==t&&e.getTime()===t.getTime()}function At(e,t){const s=Dt(t);return(e.getDay()-s+7)%7}function Dt(e){const t=Vt(e),s=Ct.firstDay[t];return void 0!==s?s:Ct.firstDay["001"]}function Mt(e){const t=Rt(e);return t.setDate(1),t}function Rt(e){const t=new Date(e.getTime());return t.setHours(0),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function jt(e){const t=new Date(e.getTime());return t.setHours(12),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Bt(e,t){const s=jt(e);return s.setDate(s.getDate()+t),Wt(e,s),s}function Ft(e,t){const s=jt(e);return s.setMonth(e.getMonth()+t),Wt(e,s),s}function Nt(){return Rt(new Date)}function Ht(e){const t=Vt(e),s=Ct.weekendEnd[t];return void 0!==s?s:Ct.weekendEnd["001"]}function zt(e){const t=Vt(e),s=Ct.weekendStart[t];return void 0!==s?s:Ct.weekendStart["001"]}function Wt(e,t){t.setHours(e.getHours()),t.setMinutes(e.getMinutes()),t.setSeconds(e.getSeconds()),t.setMilliseconds(e.getMilliseconds())}function Vt(e){const t=e?e.split("-"):null;return t?t[1]:"001"}function Yt(e){return class extends e{attributeChangedCallback(e,t,s){"date"===e?this.date=new Date(s):super.attributeChangedCallback(e,t,s)}get date(){return this[Fe].date}set date(e){Ot(e,this[Fe].date)||this[Me]({date:e})}get[ee](){return Object.assign(super[ee]||{},{date:null,locale:navigator.language})}get locale(){return this[Fe].locale}set locale(e){this[Me]({locale:e})}[Oe](e){if(super[Oe]&&super[Oe](e),e.date&&this[ke]){const e=this[Fe].date,t=new CustomEvent("date-changed",{bubbles:!0,detail:{date:e}});this.dispatchEvent(t);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:e}});this.dispatchEvent(s)}}}}function Ut(e){return class extends e{constructor(){super();!this[Ee]&&this.attachInternals&&(this[Ee]=this.attachInternals())}get[ee](){return Object.assign(super[ee]||{},{selected:!1})}[Ce](e){if(super[Ce](e),e.selected){const{selected:e}=this[Fe];C(this,"selected",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.selected){const{selected:e}=this[Fe],t=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(s)}}get selected(){return this[Fe].selected}set selected(e){this[Me]({selected:e})}}}const Gt=Yt(Ut(G)),qt=class extends Gt{get[ee](){return Object.assign(super[ee],{date:Nt(),outsideRange:!1})}[Ce](e){super[Ce](e);const{date:t}=this[Fe];if(e.date){const e=Nt(),s=t.getDay(),n=t.getDate(),r=Bt(t,1),o=Math.round(t.getTime()-e.getTime())/St;C(this,"alternate-month",Math.abs(t.getMonth()-e.getMonth())%2==1),C(this,"first-day-of-month",1===n),C(this,"first-week",n<=7),C(this,"future",t>e),C(this,"last-day-of-month",t.getMonth()!==r.getMonth()),C(this,"past",t<e),C(this,"sunday",0===s),C(this,"monday",1===s),C(this,"tuesday",2===s),C(this,"wednesday",3===s),C(this,"thursday",4===s),C(this,"friday",5===s),C(this,"saturday",6===s),C(this,"today",0===o),this[we].day.textContent=n.toString()}if(e.date||e.locale){const e=t.getDay(),{locale:s}=this[Fe],n=e===zt(s)||e===Ht(s);C(this,"weekday",!n),C(this,"weekend",n)}e.outsideRange&&C(this,"outside-range",this[Fe].outsideRange)}get outsideRange(){return this[Fe].outsideRange}set outsideRange(e){this[Me]({outsideRange:e})}get[Je](){return D.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Kt=Ut(xt),Zt=Yt(class extends Kt{}),$t=class extends Zt{get[ee](){return Object.assign(super[ee],{date:Nt(),dayPartType:qt,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[Fe].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get outsideRange(){return this[Fe].outsideRange}set outsideRange(e){this[Me]({outsideRange:e})}[Ce](e){if(super[Ce](e),e.dayPartType){const{dayPartType:e}=this[Fe];$(this[we].day,e)}const t=this[we].day;(e.dayPartType||e.date)&&(t.date=this[Fe].date),(e.dayPartType||e.locale)&&(t.locale=this[Fe].locale),(e.dayPartType||e.outsideRange)&&(t.outsideRange=this[Fe].outsideRange),(e.dayPartType||e.selected)&&(t.selected=this[Fe].selected)}get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");if(t){const e=K(this[Fe].dayPartType);e.id="day",t.replaceWith(e)}return e.content.append(A`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),e}},Jt=class extends G{get[ee](){return Object.assign(super[ee],{format:"short",locale:navigator.language})}get format(){return this[Fe].format}set format(e){this[Me]({format:e})}get locale(){return this[Fe].locale}set locale(e){this[Me]({locale:e})}[Ce](e){if(super[Ce](e),e.format||e.locale){const{format:e,locale:t}=this[Fe],s=Lt(t,{weekday:e}),n=Dt(t),r=zt(t),o=Ht(t),i=new Date(2017,0,1),a=this[Re].querySelectorAll('[part~="day-name"]');for(let e=0;e<=6;e++){const t=(n+e)%7;i.setDate(t+1);const l=t===r||t===o,c=a[e];c.toggleAttribute("weekday",!l),c.toggleAttribute("weekend",l),c.textContent=s.format(i)}}}get[Je](){return D.html`
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
    `}},Qt=Yt(G),Xt=class extends Qt{attributeChangedCallback(e,t,s){"start-date"===e?this.startDate=new Date(s):super.attributeChangedCallback(e,t,s)}dayElementForDate(e){return(this.days||[]).find((t=>Ot(t.date,e)))}get dayCount(){return this[Fe].dayCount}set dayCount(e){this[Me]({dayCount:e})}get dayPartType(){return this[Fe].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get days(){return this[Fe].days}get[ee](){const e=Nt();return Object.assign(super[ee],{date:e,dayCount:1,dayPartType:qt,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:e})}[Ce](e){if(super[Ce](e),e.days&&L(this[we].dayContainer,this[Fe].days),e.date||e.locale||e.showSelectedDay){const e=this[Fe].showSelectedDay,{date:t}=this[Fe],s=t.getDate(),n=t.getMonth(),r=t.getFullYear();(this.days||[]).forEach((t=>{const o=t.date,i=e&&o.getDate()===s&&o.getMonth()===n&&o.getFullYear()===r;t.toggleAttribute("selected",i)}))}if(e.dayCount||e.startDate){const{dayCount:e,startDate:t}=this[Fe],s=Bt(t,e);(this[Fe].days||[]).forEach((e=>{if("outsideRange"in e){const n=e.date.getTime(),r=n<t.getTime()||n>=s.getTime();e.outsideRange=r}}))}}get showCompleteWeeks(){return this[Fe].showCompleteWeeks}set showCompleteWeeks(e){this[Me]({showCompleteWeeks:e})}get showSelectedDay(){return this[Fe].showSelectedDay}set showSelectedDay(e){this[Me]({showSelectedDay:e})}get startDate(){return this[Fe].startDate}set startDate(e){Ot(this[Fe].startDate,e)||this[Me]({startDate:e})}[Ne](e,t){const s=super[Ne](e,t);if(t.dayCount||t.dayPartType||t.locale||t.showCompleteWeeks||t.startDate){const n=function(e,t){const{dayCount:s,dayPartType:n,locale:r,showCompleteWeeks:o,startDate:i}=e,a=o?function(e,t){return Rt(Bt(e,-At(e,t)))}(i,r):Rt(i);let l;if(o){c=a,u=function(e,t){return Rt(Bt(e,6-At(e,t)))}(Bt(i,s-1),r),l=Math.round((u.getTime()-c.getTime())/St)+1}else l=s;var c,u;let d=e.days?e.days.slice():[],h=a;for(let e=0;e<l;e++){const s=t||e>=d.length,o=s?K(n):d[e];o.date=new Date(h.getTime()),o.locale=r,"part"in o&&(o.part="day"),o.style.gridColumnStart="",s&&(d[e]=o),h=Bt(h,1)}l<d.length&&(d=d.slice(0,l));const p=d[0];if(p&&!o){const t=At(p.date,e.locale);p.style.gridColumnStart=t+1}return Object.freeze(d),d}(e,t.dayPartType);Object.assign(s,{days:n})}return s}get[Je](){return D.html`
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
    `}},_t=Yt(G),es=class extends _t{get[ee](){return Object.assign(super[ee],{date:Nt(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[Fe].monthFormat}set monthFormat(e){this[Me]({monthFormat:e})}[Ce](e){if(super[Ce](e),e.date||e.locale||e.monthFormat||e.yearFormat){const{date:e,locale:t,monthFormat:s,yearFormat:n}=this[Fe],r={};s&&(r.month=s),n&&(r.year=n);const o=Lt(t,r);this[we].formatted.textContent=o.format(e)}}get[Je](){return D.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[Fe].yearFormat}set yearFormat(e){this[Me]({yearFormat:e})}},ts=Yt(G);function ss(e,t,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=t,n=e.getElementById("dayNamesHeader");n&&$(n,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=t,n=e.getElementById("monthYearHeader");n&&$(n,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=t,n=e.getElementById("monthDays");n&&$(n,s)}}function ns(e){return class extends e{constructor(){super();!this[Ee]&&this.attachInternals&&(this[Ee]=this.attachInternals())}checkValidity(){return this[Ee].checkValidity()}get[ee](){return Object.assign(super[ee]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[Ee]}static get formAssociated(){return!0}get form(){return this[Ee].form}get name(){return this[Fe]?this[Fe].name:""}set name(t){"name"in e.prototype&&(super.name=t),this[Me]({name:t})}[Ce](e){if(super[Ce]&&super[Ce](e),e.name&&this.setAttribute("name",this[Fe].name),this[Ee]&&this[Ee].setValidity&&(e.valid||e.validationMessage)){const{valid:e,validationMessage:t}=this[Fe];e?this[Ee].setValidity({}):this[Ee].setValidity({customError:!0},t)}}[Oe](e){super[Oe]&&super[Oe](e),e.value&&this[Ee]&&this[Ee].setFormValue(this[Fe].value,this[Fe])}reportValidity(){return this[Ee].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[Fe].validationMessage}get validity(){return this[Ee].validity}get willValidate(){return this[Ee].willValidate}}}function rs(e){return class extends e{[ae](){if(super[ae])return super[ae]()}[le](){if(super[le])return super[le]()}[de](){if(super[de])return super[de]()}[me](){if(super[me])return super[me]()}[ge](){if(super[ge])return super[ge]()}[fe](){if(super[fe])return super[fe]()}[Te](e){let t=!1;const s=this[Fe].orientation||"both",n="horizontal"===s||"both"===s,r="vertical"===s||"both"===s;switch(e.key){case"ArrowDown":r&&(t=e.altKey?this[le]():this[ae]());break;case"ArrowLeft":!n||e.metaKey||e.altKey||(t=this[de]());break;case"ArrowRight":!n||e.metaKey||e.altKey||(t=this[me]());break;case"ArrowUp":r&&(t=e.altKey?this[ge]():this[fe]());break;case"End":t=this[le]();break;case"Home":t=this[ge]()}return t||super[Te]&&super[Te](e)||!1}}}function os(e){return class extends e{constructor(){super(),this.addEventListener("keydown",(async e=>{this[ke]=!0,this[Fe].focusVisible||this[Me]({focusVisible:!0}),this[Te](e)&&(e.preventDefault(),e.stopImmediatePropagation()),await Promise.resolve(),this[ke]=!1}))}attributeChangedCallback(e,t,s){if("tabindex"===e){let e;null===s?e=-1:(e=Number(s),isNaN(e)&&(e=this[te]?this[te]:0)),this.tabIndex=e}else super.attributeChangedCallback(e,t,s)}get[ee](){const e=this[se]?-1:0;return Object.assign(super[ee]||{},{tabIndex:e})}[Te](e){return!!super[Te]&&super[Te](e)}[Ce](e){super[Ce]&&super[Ce](e),e.tabIndex&&(this.tabIndex=this[Fe].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(e){super.tabIndex!==e&&(super.tabIndex=e),this[Ae]||this[Me]({tabIndex:e})}}}function is(e){return class extends e{connectedCallback(){const e="rtl"===getComputedStyle(this).direction;this[Me]({rightToLeft:e}),super.connectedCallback()}}}const as=kt(Yt(it(ns(rs(os(is(class extends ts{dayElementForDate(e){const t=this[we].monthDays;return t&&"dayElementForDate"in t&&t.dayElementForDate(e)}get dayNamesHeaderPartType(){return this[Fe].dayNamesHeaderPartType}set dayNamesHeaderPartType(e){this[Me]({dayNamesHeaderPartType:e})}get dayPartType(){return this[Fe].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get days(){return this[Re]?this[we].monthDays.days:[]}get daysOfWeekFormat(){return this[Fe].daysOfWeekFormat}set daysOfWeekFormat(e){this[Me]({daysOfWeekFormat:e})}get[ee](){return Object.assign(super[ee],{date:Nt(),dayNamesHeaderPartType:Jt,dayPartType:qt,daysOfWeekFormat:"short",monthDaysPartType:Xt,monthFormat:"long",monthYearHeaderPartType:es,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[Fe].monthFormat}set monthFormat(e){this[Me]({monthFormat:e})}get monthDaysPartType(){return this[Fe].monthDaysPartType}set monthDaysPartType(e){this[Me]({monthDaysPartType:e})}get monthYearHeaderPartType(){return this[Fe].monthYearHeaderPartType}set monthYearHeaderPartType(e){this[Me]({monthYearHeaderPartType:e})}[Ce](e){if(super[Ce](e),ss(this[Re],this[Fe],e),(e.dayPartType||e.monthDaysPartType)&&(this[we].monthDays.dayPartType=this[Fe].dayPartType),e.locale||e.monthDaysPartType||e.monthYearHeaderPartType||e.dayNamesHeaderPartType){const e=this[Fe].locale;this[we].monthDays.locale=e,this[we].monthYearHeader.locale=e,this[we].dayNamesHeader.locale=e}if(e.date||e.monthDaysPartType){const{date:e}=this[Fe];if(e){const t=Mt(e),s=function(e){const t=Mt(e);return t.setMonth(t.getMonth()+1),t.setDate(t.getDate()-1),t}(e).getDate();Object.assign(this[we].monthDays,{date:e,dayCount:s,startDate:t}),this[we].monthYearHeader.date=Mt(e)}}if(e.daysOfWeekFormat||e.dayNamesHeaderPartType){const{daysOfWeekFormat:e}=this[Fe];this[we].dayNamesHeader.format=e}if(e.showCompleteWeeks||e.monthDaysPartType){const{showCompleteWeeks:e}=this[Fe];this[we].monthDays.showCompleteWeeks=e}if(e.showSelectedDay||e.monthDaysPartType){const{showSelectedDay:e}=this[Fe];this[we].monthDays.showSelectedDay=e}if(e.monthFormat||e.monthYearHeaderPartType){const{monthFormat:e}=this[Fe];this[we].monthYearHeader.monthFormat=e}if(e.yearFormat||e.monthYearHeaderPartType){const{yearFormat:e}=this[Fe];this[we].monthYearHeader.yearFormat=e}}get showCompleteWeeks(){return this[Fe].showCompleteWeeks}set showCompleteWeeks(e){this[Me]({showCompleteWeeks:e})}get showSelectedDay(){return this[Fe].showSelectedDay}set showSelectedDay(e){this[Me]({showSelectedDay:e})}get[Je](){const e=D.html`
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
    `;return ss(e.content,this[Fe]),e}get yearFormat(){return this[Fe].yearFormat}set yearFormat(e){this[Me]({yearFormat:e})}}))))))),ls=class extends as{constructor(){super(),this.addEventListener("mousedown",(e=>{if(0!==e.button)return;this[ke]=!0;const t=e.composedPath()[0];if(t instanceof Node){const e=this.days,s=e[I(e,t)];s&&(this.date=s.date)}this[ke]=!1})),P(this,this)}arrowButtonNext(){const e=this[Fe].date||Nt();return this[Me]({date:Ft(e,1)}),!0}arrowButtonPrevious(){const e=this[Fe].date||Nt();return this[Me]({date:Ft(e,-1)}),!0}get[ee](){return Object.assign(super[ee],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Nt(),dayPartType:$t,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[Te](e){let t=!1;switch(e.key){case"Home":this[Me]({date:Nt()}),t=!0;break;case"PageDown":this[Me]({date:Ft(this[Fe].date,1)}),t=!0;break;case"PageUp":this[Me]({date:Ft(this[Fe].date,-1)}),t=!0}return t||super[Te]&&super[Te](e)}[ae](){return super[ae]&&super[ae](),this[Me]({date:Bt(this[Fe].date,7)}),!0}[de](){return super[de]&&super[de](),this[Me]({date:Bt(this[Fe].date,-1)}),!0}[me](){return super[me]&&super[me](),this[Me]({date:Bt(this[Fe].date,1)}),!0}[fe](){return super[fe]&&super[fe](),this[Me]({date:Bt(this[Fe].date,-7)}),!0}[Ne](e,t){const s=super[Ne](e,t);return t.date&&Object.assign(s,{value:e.date?e.date.toString():""}),s}get[Je](){const e=super[Je],t=e.content.querySelector("#monthYearHeader");this[kt.wrap](t);const s=D.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return e.content.append(s.content),e}get value(){return this.date}set value(e){this.date=e}},cs=new Set;function us(e){return class extends e{attributeChangedCallback(e,t,s){if("dark"===e){const t=w(e,s);this.dark!==t&&(this.dark=t)}else super.attributeChangedCallback(e,t,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),cs.delete(this)}get dark(){return this[Fe].dark}set dark(e){this[Me]({dark:e})}get[ee](){return Object.assign(super[ee]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[Fe].detectDarkMode}set detectDarkMode(e){"auto"!==e&&"off"!==e||this[Me]({detectDarkMode:e})}[Ce](e){if(super[Ce]&&super[Ce](e),e.dark){const{dark:e}=this[Fe];C(this,"dark",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.detectDarkMode){const{detectDarkMode:e}=this[Fe];"auto"===e?(cs.add(this),ds(this)):cs.delete(this)}}}}function ds(e){const t=function(e){const t=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(e);return t?{r:t[1],g:t[2],b:t[3]}:null}(hs(e));if(t){const s=function(e){const t=e.r/255,s=e.g/255,n=e.b/255,r=Math.max(t,s,n),o=Math.min(t,s,n);let i=0,a=0,l=(r+o)/2;const c=r-o;if(0!==c){switch(a=l>.5?c/(2-c):c/(r+o),r){case t:i=(s-n)/c+(s<n?6:0);break;case s:i=(n-t)/c+2;break;case n:i=(t-s)/c+4}i/=6}return{h:i,s:a,l}}(t).l<.5;e[Me]({dark:s})}}function hs(e){const t="rgb(255,255,255)";if(e instanceof Document)return t;const s=getComputedStyle(e).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(e.assignedSlot)return hs(e.assignedSlot);const n=e.parentNode;return n instanceof ShadowRoot?hs(n.host):n instanceof Element?hs(n):t}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{cs.forEach((e=>{ds(e)}))}));class ps extends(function(e){return class extends e{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}}}(xt)){}const ms=ps,gs=us(ms),bs=class extends gs{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},fs=class extends qt{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},ys=class extends $t{get[ee](){return Object.assign(super[ee],{dayPartType:fs})}get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},ws=class extends Jt{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},vs=class extends es{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),e}};class xs extends(us(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{arrowButtonPartType:bs})}[Ce](e){if(super[Ce](e),e.orientation||e.rightToLeft){const{orientation:e,rightToLeft:t}=this[Fe],s="vertical"===e?"rotate(90deg)":t?"rotateZ(180deg)":"";this[we].arrowIconPrevious&&(this[we].arrowIconPrevious.style.transform=s),this[we].arrowIconNext&&(this[we].arrowIconNext.style.transform=s)}if(e.dark){const{dark:e}=this[Fe],t=this[we].arrowButtonPrevious,s=this[we].arrowButtonNext;"dark"in t&&(t.dark=e),"dark"in s&&(s.dark=e)}}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="arrowButtonPrevious"]');t&&t.append(A`
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
          `);const s=e.content.querySelector('slot[name="arrowButtonNext"]');return s&&s.append(A`
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
          `),e}}}(ls))){get[ee](){return Object.assign(super[ee],{dayNamesHeaderPartType:ws,dayPartType:ys,monthYearHeaderPartType:vs})}}const Ts=xs;customElements.define("elix-calendar-month-navigator",class extends Ts{}),Symbol("generatedId");const Ps={a:"link",article:"region",button:"button",h1:"sectionhead",h2:"sectionhead",h3:"sectionhead",h4:"sectionhead",h5:"sectionhead",h6:"sectionhead",hr:"sectionhead",iframe:"region",link:"link",menu:"menu",ol:"list",option:"option",output:"liveregion",progress:"progressbar",select:"select",table:"table",td:"td",textarea:"textbox",th:"th",ul:"list"};function Is(e){return class extends e{attributeChangedCallback(e,t,s){if("current-index"===e)this.currentIndex=Number(s);else if("current-item-required"===e){const t=w(e,s);this.currentItemRequired!==t&&(this.currentItemRequired=t)}else if("cursor-operations-wrap"===e){const t=w(e,s);this.cursorOperationsWrap!==t&&(this.cursorOperationsWrap=t)}else super.attributeChangedCallback(e,t,s)}get currentIndex(){const{items:e,currentIndex:t}=this[Fe];return e&&e.length>0?t:-1}set currentIndex(e){isNaN(e)||this[Me]({currentIndex:e})}get currentItem(){const{items:e,currentIndex:t}=this[Fe];return e&&e[t]}set currentItem(e){const{items:t}=this[Fe];if(!t)return;const s=t.indexOf(e);s>=0&&this[Me]({currentIndex:s})}get currentItemRequired(){return this[Fe].currentItemRequired}set currentItemRequired(e){this[Me]({currentItemRequired:e})}get cursorOperationsWrap(){return this[Fe].cursorOperationsWrap}set cursorOperationsWrap(e){this[Me]({cursorOperationsWrap:e})}goFirst(){return super.goFirst&&super.goFirst(),this[ce]()}goLast(){return super.goLast&&super.goLast(),this[ue]()}goNext(){return super.goNext&&super.goNext(),this[he]()}goPrevious(){return super.goPrevious&&super.goPrevious(),this[pe]()}[Oe](e){if(super[Oe]&&super[Oe](e),e.currentIndex&&this[ke]){const{currentIndex:e}=this[Fe],t=new CustomEvent("current-index-changed",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("currentindexchange",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(s)}}}}function Es(e){const t=e[Re],s=t&&t.querySelector("slot:not([name])");return s&&s.parentNode instanceof Element&&function(e){for(const t of k(e))if(t instanceof HTMLElement&&ks(t))return t;return null}(s.parentNode)||e}function ks(e){const t=getComputedStyle(e),s=t.overflowX,n=t.overflowY;return"scroll"===s||"auto"===s||"scroll"===n||"auto"===n}function Cs(e){return class extends e{[Oe](e){super[Oe]&&super[Oe](e),e.currentItem&&this.scrollCurrentItemIntoView()}scrollCurrentItemIntoView(){super.scrollCurrentItemIntoView&&super.scrollCurrentItemIntoView();const{currentItem:e,items:t}=this[Fe];if(!e||!t)return;const s=this[De].getBoundingClientRect(),n=e.getBoundingClientRect(),r=n.bottom-s.bottom,o=n.left-s.left,i=n.right-s.right,a=n.top-s.top,l=this[Fe].orientation||"both";"horizontal"!==l&&"both"!==l||(i>0?this[De].scrollLeft+=i:o<0&&(this[De].scrollLeft+=Math.ceil(o))),"vertical"!==l&&"both"!==l||(r>0?this[De].scrollTop+=r:a<0&&(this[De].scrollTop+=Math.ceil(a)))}get[De](){return super[De]||Es(this)}}}function Ss(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{canGoDown:null,canGoLeft:null,canGoRight:null,canGoUp:null})}[ae](){return super[ae]&&super[ae](),this[he]()}[le](){return super[le]&&super[le](),this[ue]()}[de](){return super[de]&&super[de](),this[Fe]&&this[Fe].rightToLeft?this[he]():this[pe]()}[me](){return super[me]&&super[me](),this[Fe]&&this[Fe].rightToLeft?this[pe]():this[he]()}[ge](){return super[ge]&&super[ge](),this[ce]()}[fe](){return super[fe]&&super[fe](),this[pe]()}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.canGoNext||t.canGoPrevious||t.languageDirection||t.orientation||t.rightToLeft){const{canGoNext:t,canGoPrevious:n,orientation:r,rightToLeft:o}=e,i="horizontal"===r||"both"===r,a="vertical"===r||"both"===r,l=a&&t,c=!!i&&(o?t:n),u=!!i&&(o?n:t),d=a&&n;Object.assign(s,{canGoDown:l,canGoLeft:c,canGoRight:u,canGoUp:d})}return s}}}function Ls(e){return class extends e{get items(){return this[Fe]?this[Fe].items:null}[Oe](e){if(super[Oe]&&super[Oe](e),!this[re]&&e.items&&this[ke]){const e=new CustomEvent("items-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("itemschange",{bubbles:!0});this.dispatchEvent(t)}}}}function Os(e){return class extends e{[X](e,t={}){const s=void 0!==t.direction?t.direction:1,n=void 0!==t.index?t.index:e.currentIndex,r=void 0!==t.wrap?t.wrap:e.cursorOperationsWrap,{items:o}=e,i=o?o.length:0;if(0===i)return-1;if(r){let t=(n%i+i)%i;const r=((t-s)%i+i)%i;for(;t!==r;){if(!e.availableItemFlags||e.availableItemFlags[t])return t;t=((t+s)%i+i)%i}}else for(let t=n;t>=0&&t<i;t+=s)if(!e.availableItemFlags||e.availableItemFlags[t])return t;return-1}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,desiredCurrentIndex:null,currentItem:null,currentItemRequired:!1,cursorOperationsWrap:!1})}[ce](){return super[ce]&&super[ce](),As(this,0,1)}[ue](){return super[ue]&&super[ue](),As(this,this[Fe].items.length-1,-1)}[he](){super[he]&&super[he]();const{currentIndex:e,items:t}=this[Fe];return As(this,e<0&&t?0:e+1,1)}[pe](){super[pe]&&super[pe]();const{currentIndex:e,items:t}=this[Fe];return As(this,e<0&&t?t.length-1:e-1,-1)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.availableItemFlags||t.items||t.currentIndex||t.currentItemRequired){const{currentIndex:n,desiredCurrentIndex:r,currentItem:o,currentItemRequired:i,items:a}=e,l=a?a.length:0;let c,u=r;if(t.items&&!t.currentIndex&&o&&l>0&&a[n]!==o){const e=a.indexOf(o);e>=0&&(u=e)}else t.currentIndex&&(n<0&&null!==o||n>=0&&(0===l||a[n]!==o)||null===r)&&(u=n);i&&u<0&&(u=0),u<0?(u=-1,c=-1):0===l?c=-1:(c=Math.max(Math.min(l-1,u),0),c=this[X](e,{direction:1,index:c,wrap:!1}),c<0&&(c=this[X](e,{direction:-1,index:c-1,wrap:!1})));const d=a&&a[c]||null;Object.assign(s,{currentIndex:c,desiredCurrentIndex:u,currentItem:d})}return s}}}function As(e,t,s){const n=e[X](e[Fe],{direction:s,index:t});if(n<0)return!1;const r=e[Fe].currentIndex!==n;return r&&e[Me]({currentIndex:n}),r}const Ds=["applet","basefont","embed","font","frame","frameset","isindex","keygen","link","multicol","nextid","noscript","object","param","script","style","template","noembed"];function Ms(e){return e.getAttribute("aria-label")||e.getAttribute("alt")||e.innerText||e.textContent||""}function Rs(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{texts:null})}[ie](e){return super[ie]?super[ie](e):Ms(e)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items){const{items:t}=e,n=function(e,t){return e?Array.from(e,(e=>t(e))):null}(t,this[ie]);n&&(Object.freeze(n),Object.assign(s,{texts:n}))}return s}}}function js(e){return class extends e{[Te](e){let t=!1;if("horizontal"!==this.orientation)switch(e.key){case"PageDown":t=this.pageDown();break;case"PageUp":t=this.pageUp()}return t||super[Te]&&super[Te](e)}get orientation(){return super.orientation||this[Fe]&&this[Fe].orientation||"both"}pageDown(){return super.pageDown&&super.pageDown(),Fs(this,!0)}pageUp(){return super.pageUp&&super.pageUp(),Fs(this,!1)}get[De](){return super[De]||Es(this)}}}function Bs(e,t,s){const n=e[Fe].items,r=s?0:n.length-1,o=s?n.length:0,i=s?1:-1;let a,l,c=null;const{availableItemFlags:u}=e[Fe];for(a=r;a!==o;a+=i)if((!u||u[a])&&(l=n[a].getBoundingClientRect(),l.top<=t&&t<=l.bottom)){c=n[a];break}if(!c||!l)return null;const d=getComputedStyle(c),h=d.paddingTop?parseFloat(d.paddingTop):0,p=d.paddingBottom?parseFloat(d.paddingBottom):0,m=l.top+h,g=m+c.clientHeight-h-p;return s&&m<=t||!s&&g>=t?a:a-i}function Fs(e,t){const s=e[Fe].items,n=e[Fe].currentIndex,r=e[De].getBoundingClientRect(),o=Bs(e,t?r.bottom:r.top,t);let i;if(o&&n===o){const r=s[n].getBoundingClientRect(),o=e[De].clientHeight;i=Bs(e,t?r.bottom+o:r.top-o,t)}else i=o;if(!i){const n=t?s.length-1:0;i=e[X]?e[X](e[Fe],{direction:t?-1:1,index:n}):n}const a=i!==n;if(a){const t=e[ke];e[ke]=!0,e[Me]({currentIndex:i}),e[ke]=t}return a}const Ns=Symbol("typedPrefix"),Hs=Symbol("prefixTimeout");function zs(e){return class extends e{constructor(){super(),Vs(this)}[be](e){if(super[be]&&super[be](e),null==e||0===e.length)return!1;const t=e.toLowerCase(),s=this[Fe].texts.findIndex((s=>s.substr(0,e.length).toLowerCase()===t));if(s>=0){const e=this[Fe].currentIndex;return this[Me]({currentIndex:s}),this[Fe].currentIndex!==e}return!1}[Te](e){let t;switch(e.key){case"Backspace":!function(e){const t=e,s=t[Ns]?t[Ns].length:0;s>0&&(t[Ns]=t[Ns].substr(0,s-1)),e[be](t[Ns]),Ys(e)}(this),t=!0;break;case"Escape":Vs(this);break;default:e.ctrlKey||e.metaKey||e.altKey||1!==e.key.length||function(e,t){const s=e,n=s[Ns]||"";s[Ns]=n+t,e[be](s[Ns]),Ys(e)}(this,e.key)}return t||super[Te]&&super[Te](e)}}}function Ws(e){const t=e;t[Hs]&&(clearTimeout(t[Hs]),t[Hs]=!1)}function Vs(e){e[Ns]="",Ws(e)}function Ys(e){Ws(e),e[Hs]=setTimeout((()=>{Vs(e)}),1e3)}function Us(e){return class extends e{get[_](){const e=this[Re]&&this[Re].querySelector("slot:not([name])");return this[Re]&&e||console.warn(`SlotContentMixin expects ${this.constructor.name} to define a shadow tree that includes a default (unnamed) slot.\nSee https://elix.org/documentation/SlotContentMixin.`),e}get[ee](){return Object.assign(super[ee]||{},{content:null})}[Oe](e){if(super[Oe]&&super[Oe](e),this[re]){const e=this[_];e&&e.addEventListener("slotchange",(async()=>{this[ke]=!0;const t=e.assignedNodes({flatten:!0});Object.freeze(t),this[Me]({content:t}),await Promise.resolve(),this[ke]=!1}))}}}}function Gs(e){return function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{items:null})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.content){const t=e.content,n=t?Array.prototype.filter.call(t,(e=>{return(t=e)instanceof Element&&(!t.localName||Ds.indexOf(t.localName)<0);var t})):null;n&&Object.freeze(n),Object.assign(s,{items:n})}return s}}}(Us(e))}function qs(e){return class extends e{constructor(){super(),this.addEventListener("mousedown",(e=>{0===e.button&&(this[ke]=!0,this[$e](e),this[ke]=!1)}))}[Ce](e){super[Ce]&&super[Ce](e),this[re]&&Object.assign(this.style,{touchAction:"manipulation",mozUserSelect:"none",msUserSelect:"none",webkitUserSelect:"none",userSelect:"none"})}[$e](e){const t=e.composedPath?e.composedPath()[0]:e.target,{items:s,currentItemRequired:n}=this[Fe];if(s&&t instanceof Node){const r=I(s,t),o=r>=0?s[r]:null;(o&&!o.disabled||!o&&!n)&&(this[Me]({currentIndex:r}),e.stopPropagation())}}}}const Ks=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"menuitem",role:e.role||"menu"})}get itemRole(){return this[Fe].itemRole}set itemRole(e){this[Me]({itemRole:e})}[Ce](e){super[Ce]&&super[Ce](e);const t=this[Fe].items;if((e.items||e.itemRole)&&t){const{itemRole:e}=this[Fe];t.forEach((t=>{e===Ps[t.localName]?t.removeAttribute("role"):t.setAttribute("role",e)}))}if(e.role){const{role:e}=this[Fe];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}(Is(Cs(lt(Ss(Ls(Os(Rs(rs(os(js(zs(is(Gs(qs(G))))))))))))))),Zs=class extends Ks{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,highlightCurrentItem:!0,orientation:"vertical",currentItemFocused:!1})}async flashCurrentItem(){const e=this[Fe].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Me]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Me]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[Ce](e){super[Ce](e),this[re]&&(this.addEventListener("disabledchange",(e=>{this[ke]=!0;const t=e.target,{items:s}=this[Fe],n=null===s?-1:s.indexOf(t);if(n>=0){const e=this[Fe].availableItemFlags.slice();e[n]=!t.disabled,this[Me]({availableItemFlags:e})}this[ke]=!1})),"PointerEvent"in window?this.addEventListener("pointerdown",(e=>this[$e](e))):this.addEventListener("touchstart",(e=>this[$e](e))),this.removeAttribute("tabindex"));const{currentIndex:t,items:s}=this[Fe];if((e.items||e.currentIndex||e.highlightCurrentItem)&&s){const{highlightCurrentItem:e}=this[Fe];s.forEach(((s,n)=>{s.toggleAttribute("current",e&&n===t)}))}(e.items||e.currentIndex||e.currentItemFocused||e.focusVisible)&&s&&s.forEach(((e,s)=>{const n=s===t,r=t<0&&0===s;this[Fe].currentItemFocused?n||r||e.removeAttribute("tabindex"):(n||r)&&(e.tabIndex=0)}))}[Oe](e){if(super[Oe](e),!this[re]&&e.currentIndex&&!this[Fe].currentItemFocused){const{currentItem:e}=this[Fe];(e instanceof HTMLElement?e:this).focus(),this[Me]({currentItemFocused:!0})}}get[De](){return this[we].content}[Ne](e,t){const s=super[Ne](e,t);if(t.currentIndex&&Object.assign(s,{currentItemFocused:!1}),t.items){const{items:t}=e,n=null===t?null:t.map((e=>!e.disabled));Object.assign(s,{availableItemFlags:n})}return s}get[Je](){return D.html`
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
    `}},$s=class extends Zs{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}};customElements.define("elix-menu",class extends $s{});const Js=Symbol("documentMouseupListener");async function Qs(e){const t=this,s=t[Re].elementsFromPoint(e.clientX,e.clientY);if(t.opened){const e=s.indexOf(t[we].source)>=0,n=t[we].popup,r=s.indexOf(n)>=0,o=n.frame&&s.indexOf(n.frame)>=0;e?t[Fe].dragSelect&&(t[ke]=!0,t[Me]({dragSelect:!1}),t[ke]=!1):r||o||(t[ke]=!0,await t.close(),t[ke]=!1)}}function Xs(e){e[Fe].opened&&e.isConnected?e[Js]||(e[Js]=Qs.bind(e),document.addEventListener("mouseup",e[Js])):e[Js]&&(document.removeEventListener("mouseup",e[Js]),e[Js]=null)}function _s(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{disabled:!1})}get disabled(){return this[Fe].disabled}set disabled(e){this[Me]({disabled:e})}[Oe](e){if(super[Oe]&&super[Oe](e),e.disabled&&(this.toggleAttribute("disabled",this.disabled),this[ke])){const e=new CustomEvent("disabled-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("disabledchange",{bubbles:!0});this.dispatchEvent(t)}}}}const en=Symbol("closePromise"),tn=Symbol("closeResolve");function sn(e){return class extends e{attributeChangedCallback(e,t,s){if("opened"===e){const t=w(e,s);this.opened!==t&&(this.opened=t)}else super.attributeChangedCallback(e,t,s)}async close(e){super.close&&await super.close(),this[Me]({closeResult:e}),await this.toggle(!1)}get closed(){return this[Fe]&&!this[Fe].opened}get closeFinished(){return this[Fe].openCloseEffects?"close"===this[Fe].effect&&"after"===this[Fe].effectPhase:this.closed}get closeResult(){return this[Fe].closeResult}get[ee](){const e={closeResult:null,opened:!1};return this[Be]&&Object.assign(e,{effect:"close",effectPhase:"after",openCloseEffects:!0}),Object.assign(super[ee]||{},e)}async open(){super.open&&await super.open(),this[Me]({closeResult:void 0}),await this.toggle(!0)}get opened(){return this[Fe]&&this[Fe].opened}set opened(e){this[Me]({closeResult:void 0}),this.toggle(e)}[Oe](e){if(super[Oe]&&super[Oe](e),e.opened&&this[ke]){const e=new CustomEvent("opened-changed",{bubbles:!0,detail:{closeResult:this[Fe].closeResult,opened:this[Fe].opened}});this.dispatchEvent(e);const t=new CustomEvent("openedchange",{bubbles:!0,detail:{closeResult:this[Fe].closeResult,opened:this[Fe].opened}});if(this.dispatchEvent(t),this[Fe].opened){const e=new CustomEvent("opened",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("open",{bubbles:!0});this.dispatchEvent(t)}else{const e=new CustomEvent("closed",{bubbles:!0,detail:{closeResult:this[Fe].closeResult}});this.dispatchEvent(e);const t=new CustomEvent("close",{bubbles:!0,detail:{closeResult:this[Fe].closeResult}});this.dispatchEvent(t)}}const t=this[tn];this.closeFinished&&t&&(this[tn]=null,this[en]=null,t(this[Fe].closeResult))}async toggle(e=!this.opened){if(super.toggle&&await super.toggle(e),e!==this[Fe].opened){const t={opened:e};this[Fe].openCloseEffects&&(t.effect=e?"open":"close","after"===this[Fe].effectPhase&&(t.effectPhase="before")),await this[Me](t)}}whenClosed(){return this[en]||(this[en]=new Promise((e=>{this[tn]=e}))),this[en]}}}function nn(e){return class extends e{get[ee](){return Object.assign(super[ee],{role:null})}[Ce](e){if(super[Ce]&&super[Ce](e),e.role){const{role:e}=this[Fe];e?this.setAttribute("role",e):this.removeAttribute("role")}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}const rn=nn(G),on=class extends rn{get[ee](){return Object.assign(super[ee],{role:"none"})}get[Je](){return D.html`
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
    `}},an=class extends G{get[Je](){return D.html`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }
      </style>
      <slot></slot>
    `}},ln=Symbol("appendedToDocument"),cn=Symbol("assignedZIndex"),un=Symbol("restoreFocusToElement");function dn(e){const t=function(){const e=document.body.querySelectorAll("*"),t=Array.from(e,(e=>{const t=getComputedStyle(e);let s=0;if("static"!==t.position&&"auto"!==t.zIndex){const e=t.zIndex?parseInt(t.zIndex):0;s=isNaN(e)?0:e}return s}));return Math.max(...t)}()+1;e[cn]=t,e.style.zIndex=t.toString()}function hn(e){const t=getComputedStyle(e).zIndex,s=e.style.zIndex,n=!isNaN(parseInt(s));if("auto"===t)return n;if("0"===t&&!n){const t=e.assignedSlot||(e instanceof ShadowRoot?e.host:e.parentNode);if(!(t instanceof HTMLElement))return!0;if(!hn(t))return!1}return!0}const pn=sn(function(e){return class extends e{get autoFocus(){return this[Fe].autoFocus}set autoFocus(e){this[Me]({autoFocus:e})}get[ee](){return Object.assign(super[ee]||{},{autoFocus:!0,persistent:!1})}async open(){this[Fe].persistent||this.isConnected||(this[ln]=!0,document.body.append(this)),super.open&&await super.open()}[Ce](e){if(super[Ce]&&super[Ce](e),this[re]&&this.addEventListener("blur",(e=>{const t=e.relatedTarget||document.activeElement;t instanceof HTMLElement&&(x(this,t)||(this.opened?this[un]=t:(t.focus(),this[un]=null)))})),(e.effectPhase||e.opened||e.persistent)&&!this[Fe].persistent){const e=void 0===this.closeFinished?this.closed:this.closeFinished;this.style.display=e?"none":"",e?this[cn]&&(this.style.zIndex="",this[cn]=null):this[cn]?this.style.zIndex=this[cn]:hn(this)||dn(this)}}[Oe](e){if(super[Oe]&&super[Oe](e),this[re]&&this[Fe].persistent&&!hn(this)&&dn(this),e.opened&&this[Fe].autoFocus)if(this[Fe].opened){this[un]||document.activeElement===document.body||(this[un]=document.activeElement);const e=T(this);e&&e.focus()}else this[un]&&(this[un].focus(),this[un]=null);!this[re]&&!this[Fe].persistent&&this.closeFinished&&this[ln]&&(this[ln]=!1,this.parentNode&&this.parentNode.removeChild(this))}}}(Us(G)));function mn(e,t,s){if(!s||s.backdropPartType){const{backdropPartType:s}=t,n=e.getElementById("backdrop");n&&$(n,s)}if(!s||s.framePartType){const{framePartType:s}=t,n=e.getElementById("frame");n&&$(n,s)}}const gn=class extends pn{get backdrop(){return this[we]&&this[we].backdrop}get backdropPartType(){return this[Fe].backdropPartType}set backdropPartType(e){this[Me]({backdropPartType:e})}get[ee](){return Object.assign(super[ee],{backdropPartType:on,framePartType:an})}get frame(){return this[we].frame}get framePartType(){return this[Fe].framePartType}set framePartType(e){this[Me]({framePartType:e})}[Ce](e){super[Ce](e),mn(this[Re],this[Fe],e)}[Oe](e){super[Oe](e),e.opened&&this[Fe].content&&this[Fe].content.forEach((e=>{e[Q]&&e[Q]()}))}get[Je](){const e=D.html`
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
    `;return mn(e.content,this[Fe]),e}},bn=Symbol("implicitCloseListener");async function fn(e){const t=this,s=e.relatedTarget||document.activeElement;s instanceof Element&&!x(t,s)&&(t[ke]=!0,await t.close({canceled:"window blur"}),t[ke]=!1)}async function yn(e){const t=this,s="resize"!==e.type||t[Fe].closeOnWindowResize;!E(t,e)&&s&&(t[ke]=!0,await t.close({canceled:"window "+e.type}),t[ke]=!1)}const wn=os(function(e){return class extends e{constructor(){super(),this.addEventListener("blur",fn.bind(this))}get closeOnWindowResize(){return this[Fe].closeOnWindowResize}set closeOnWindowResize(e){this[Me]({closeOnWindowResize:e})}get[ee](){return Object.assign(super[ee]||{},{closeOnWindowResize:!0,role:"alert"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super.keydown&&super.keydown(e)||!1}[Ce](e){if(super[Ce]&&super[Ce](e),e.role){const{role:e}=this[Fe];this.setAttribute("role",e)}}[Oe](e){var t;super[Oe]&&super[Oe](e),e.opened&&(this.opened?("requestIdleCallback"in window?window.requestIdleCallback:setTimeout)((()=>{var e;this.opened&&((e=this)[bn]=yn.bind(e),window.addEventListener("blur",e[bn]),window.addEventListener("resize",e[bn]),window.addEventListener("scroll",e[bn]))})):(t=this)[bn]&&(window.removeEventListener("blur",t[bn]),window.removeEventListener("resize",t[bn]),window.removeEventListener("scroll",t[bn]),t[bn]=null))}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}(gn));async function vn(e){const t=this;t[ke]=!0,await t.close({canceled:"mousedown outside"}),t[ke]=!1,e.preventDefault(),e.stopPropagation()}const xn=class extends wn{[Ce](e){super[Ce](e),e.backdropPartType&&(this[we].backdrop.addEventListener("mousedown",vn.bind(this)),"PointerEvent"in window||this[we].backdrop.addEventListener("touchend",vn))}},Tn=Symbol("resizeListener"),Pn=_s(it(is(sn(G))));function In(e){const t=window.innerHeight,s=window.innerWidth,n=e[we].popup.getBoundingClientRect(),r=e.getBoundingClientRect(),o=n.height,i=n.width,{horizontalAlign:a,popupPosition:l,rightToLeft:c}=e[Fe],u=r.top,d=Math.ceil(t-r.bottom),h=r.right,p=Math.ceil(s-r.left),m=o<=u,g=o<=d,b="below"===l,f=b&&(g||d>=u)||!b&&!m&&d>=u,y=f&&g||!f&&m?null:f?d:u,w=f?"below":"above";let v,x,T;if("stretch"===a)v=0,x=0,T=null;else{const e="left"===a||(c?"end"===a:"start"===a),t=e&&(i<=p||p>=h)||!e&&!(i<=h)&&p>=h;v=t?0:null,x=t?null:0,T=t&&p||!t&&h?null:t?p:h}e[Me]({calculatedFrameMaxHeight:y,calculatedFrameMaxWidth:T,calculatedPopupLeft:v,calculatedPopupPosition:w,calculatedPopupRight:x,popupMeasured:!0})}function En(e,t,s){if(!s||s.popupPartType){const{popupPartType:s}=t,n=e.getElementById("popup");n&&$(n,s)}if(!s||s.sourcePartType){const{sourcePartType:s}=t,n=e.getElementById("source");n&&$(n,s)}}const kn=_s(G),Cn=class extends kn{get[ee](){return Object.assign(super[ee],{direction:"down"})}get direction(){return this[Fe].direction}set direction(e){this[Me]({direction:e})}[Ce](e){if(super[Ce](e),e.direction){const{direction:e}=this[Fe];this[we].downIcon.style.display="down"===e?"block":"none",this[we].upIcon.style.display="up"===e?"block":"none"}}get[Je](){return D.html`
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
    `}};function Sn(e,t,s){if(!s||s.popupTogglePartType){const{popupTogglePartType:s}=t,n=e.getElementById("popupToggle");n&&$(n,s)}}const Ln=lt(os(function(e){return class extends e{connectedCallback(){super.connectedCallback(),Xs(this)}get[ee](){return Object.assign(super[ee],{dragSelect:!0})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Xs(this)}[Oe](e){super[Oe](e),e.opened&&Xs(this)}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&e.opened&&Object.assign(s,{dragSelect:!0}),s}}}(function(e){return class extends e{get[ee](){return Object.assign(super[ee],{popupTogglePartType:Cn})}get popupTogglePartType(){return this[Fe].popupTogglePartType}set popupTogglePartType(e){this[Me]({popupTogglePartType:e})}[Ce](e){if(super[Ce](e),Sn(this[Re],this[Fe],e),e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[Fe],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.disabled){const{disabled:e}=this[Fe];this[we].popupToggle.disabled=e}}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="source"]');return t&&t.append(A`
        <div
          id="popupToggle"
          part="popup-toggle"
          exportparts="toggle-icon, down-icon, up-icon"
          tabindex="-1"
        >
          <slot name="toggle-icon"></slot>
        </div>
      `),Sn(e.content,this[Fe]),e.content.append(A`
      <style>
        [part~="popup-toggle"] {
          outline: none;
        }

        [part~="source"] {
          align-items: center;
          display: flex;
        }
      </style>
    `),e}}}(class extends Pn{get[ee](){return Object.assign(super[ee],{ariaHasPopup:"true",horizontalAlign:"start",popupHeight:null,popupMeasured:!1,popupPosition:"below",popupPartType:xn,popupWidth:null,roomAbove:null,roomBelow:null,roomLeft:null,roomRight:null,sourcePartType:"div"})}get[ve](){return this[we].source}get frame(){return this[we].popup.frame}get horizontalAlign(){return this[Fe].horizontalAlign}set horizontalAlign(e){this[Me]({horizontalAlign:e})}[Ce](e){if(super[Ce](e),En(this[Re],this[Fe],e),this[re]||e.ariaHasPopup){const{ariaHasPopup:e}=this[Fe];null===e?this[ve].removeAttribute("aria-haspopup"):this[ve].setAttribute("aria-haspopup",this[Fe].ariaHasPopup)}if(e.popupPartType&&(this[we].popup.addEventListener("open",(()=>{this.opened||(this[ke]=!0,this.open(),this[ke]=!1)})),this[we].popup.addEventListener("close",(e=>{if(!this.closed){this[ke]=!0;const t=e.detail.closeResult;this.close(t),this[ke]=!1}}))),e.horizontalAlign||e.popupMeasured||e.rightToLeft){const{calculatedFrameMaxHeight:e,calculatedFrameMaxWidth:t,calculatedPopupLeft:s,calculatedPopupPosition:n,calculatedPopupRight:r,popupMeasured:o}=this[Fe],i="below"===n,a=i?null:0,l=o?null:0,c=o?"absolute":"fixed",u=s,d=r,h=this[we].popup;Object.assign(h.style,{bottom:a,left:u,opacity:l,position:c,right:d});const p=h.frame;Object.assign(p.style,{maxHeight:e?e+"px":null,maxWidth:t?t+"px":null}),this[we].popupContainer.style.top=i?"":"0"}if(e.opened){const{opened:e}=this[Fe];this[we].popup.opened=e}if(e.disabled&&"disabled"in this[we].source){const{disabled:e}=this[Fe];this[we].source.disabled=e}}[Oe](e){var t;super[Oe](e),e.opened?this.opened?(t=this,setTimeout((()=>{t.opened&&(In(t),function(e){const t=e;t[Tn]=()=>{In(e)},window.addEventListener("resize",t[Tn])}(t))}))):function(e){const t=e;t[Tn]&&(window.removeEventListener("resize",t[Tn]),t[Tn]=null)}(this):this.opened&&!this[Fe].popupMeasured&&In(this)}get popupPosition(){return this[Fe].popupPosition}set popupPosition(e){this[Me]({popupPosition:e})}get popupPartType(){return this[Fe].popupPartType}set popupPartType(e){this[Me]({popupPartType:e})}get sourcePartType(){return this[Fe].sourcePartType}set sourcePartType(e){this[Me]({sourcePartType:e})}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&!e.opened&&Object.assign(s,{calculatedFrameMaxHeight:null,calculatedFrameMaxWidth:null,calculatedPopupLeft:null,calculatedPopupPosition:null,calculatedPopupRight:null,popupMeasured:!1}),s}get[Je](){const e=super[Je];return e.content.append(A`
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
    `),En(e.content,this[Fe]),e}})))),On=class extends Ln{get[ee](){return Object.assign(super[ee],{sourcePartType:"button"})}[Te](e){let t;switch(e.key){case" ":case"ArrowDown":case"ArrowUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0)}if(t=super[Te]&&super[Te](e),!t&&this.opened&&!e.metaKey&&!e.altKey)switch(e.key){case"ArrowDown":case"ArrowLeft":case"ArrowRight":case"ArrowUp":case"End":case"Home":case"PageDown":case"PageUp":case" ":t=!0}return t}[Ce](e){if(super[Ce](e),this[re]&&this[we].source.addEventListener("focus",(async e=>{const t=E(this[we].popup,e),s=null!==this[Fe].popupHeight;!t&&this.opened&&s&&(this[ke]=!0,await this.close(),this[ke]=!1)})),e.opened){const{opened:e}=this[Fe];this.toggleAttribute("opened",e),this[we].source.setAttribute("aria-expanded",e.toString())}e.sourcePartType&&this[we].source.addEventListener("mousedown",(e=>{if(this.disabled)return void e.preventDefault();const t=e;t.button&&0!==t.button||(setTimeout((()=>{this.opened||(this[ke]=!0,this.open(),this[ke]=!1)})),e.stopPropagation())})),e.popupPartType&&this[we].popup.removeAttribute("tabindex")}get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},An=Symbol("documentMousemoveListener");function Dn(e){return class extends e{connectedCallback(){super.connectedCallback(),Rn(this)}get[ee](){return Object.assign(super[ee],{currentIndex:-1,hasHoveredOverItemSinceOpened:!1,popupList:null})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Rn(this)}[Te](e){let t=!1;switch(e.key){case"Enter":this.opened&&(jn(this),t=!0)}return t||super[Te]&&super[Te](e)||!1}[Ce](e){if(super[Ce]&&super[Ce](e),e.popupList){const{popupList:e}=this[Fe];e&&(e.addEventListener("mouseup",(async e=>{const t=this[Fe].currentIndex;this[Fe].dragSelect||t>=0?(e.stopPropagation(),this[ke]=!0,await jn(this),this[ke]=!1):e.stopPropagation()})),e.addEventListener("currentindexchange",(e=>{this[ke]=!0;const t=e;this[Me]({currentIndex:t.detail.currentIndex}),this[ke]=!1})))}if(e.currentIndex||e.popupList){const{currentIndex:e,popupList:t}=this[Fe];t&&"currentIndex"in t&&(t.currentIndex=e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.opened){if(this[Fe].opened){const{popupList:e}=this[Fe];e.scrollCurrentItemIntoView&&setTimeout((()=>{e.scrollCurrentItemIntoView()}))}Rn(this)}}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};return t.opened&&e.opened&&Object.assign(s,{hasHoveredOverItemSinceOpened:!1}),s}}}function Mn(e){const t=this,{hasHoveredOverItemSinceOpened:s,opened:n}=t[Fe];if(n){const n=e.composedPath?e.composedPath()[0]:e.target;if(n&&n instanceof Node){const e=t.items,r=I(e,n),o=e[r],i=o&&!o.disabled?r:-1;(s||i>=0)&&i!==t[Fe].currentIndex&&(t[ke]=!0,t[Me]({currentIndex:i}),i>=0&&!s&&t[Me]({hasHoveredOverItemSinceOpened:!0}),t[ke]=!1)}}}function Rn(e){e[Fe].opened&&e.isConnected?e[An]||(e[An]=Mn.bind(e),document.addEventListener("mousemove",e[An])):e[An]&&(document.removeEventListener("mousemove",e[An]),e[An]=null)}async function jn(e){const t=e[ke],s=e[Fe].currentIndex>=0,n=s?e.items[e[Fe].currentIndex]:void 0,r=e[Fe].popupList;s&&"flashCurrentItem"in r&&await r.flashCurrentItem();const o=e[ke];e[ke]=t,await e.close(n),e[ke]=o}const Bn=Dn(On);function Fn(e,t,s){if(!s||s.menuPartType){const{menuPartType:s}=t,n=e.getElementById("menu");n&&$(n,s)}}const Nn=class extends Bn{get[ee](){return Object.assign(super[ee],{menuPartType:Zs})}get items(){const e=this[we]&&this[we].menu;return e?e.items:null}get menuPartType(){return this[Fe].menuPartType}set menuPartType(e){this[Me]({menuPartType:e})}[Ce](e){super[Ce](e),Fn(this[Re],this[Fe],e),e.menuPartType&&(this[we].menu.addEventListener("blur",(async e=>{const t=e.relatedTarget||document.activeElement;this.opened&&!x(this[we].menu,t)&&(this[ke]=!0,await this.close(),this[ke]=!1)})),this[we].menu.addEventListener("mousedown",(e=>{0===e.button&&this.opened&&(e.stopPropagation(),e.preventDefault())})))}[Oe](e){super[Oe](e),e.menuPartType&&this[Me]({popupList:this[we].menu})}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&!e.opened&&Object.assign(s,{currentIndex:-1}),s}get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <div id="menu" part="menu">
          <slot></slot>
        </div>
      `),Fn(e.content,this[Fe]),e.content.append(A`
      <style>
        [part~="menu"] {
          max-height: 100%;
        }
      </style>
    `),e}},Hn=class extends ms{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          [part~="inner"] {
            background: #eee;
            border: 1px solid #ccc;
            padding: 0.25em 0.5em;
          }
        </style>
      `),e}},zn=class extends Cn{get[Je](){const e=super[Je],t=e.content.getElementById("downIcon"),s=A`
      <svg
        id="downIcon"
        part="toggle-icon down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 0 l5 5 5 -5 z" />
      </svg>
    `.firstElementChild;t&&s&&Z(t,s);const n=e.content.getElementById("upIcon"),r=A`
      <svg
        id="upIcon"
        part="toggle-icon up-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 5 l5 -5 5 5 z" />
      </svg>
    `.firstElementChild;return n&&r&&Z(n,r),e.content.append(A`
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
      `),e}},Wn=class extends on{},Vn=class extends an{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 0px 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
          }
        </style>
      `),e}},Yn=class extends xn{get[ee](){return Object.assign(super[ee],{backdropPartType:Wn,framePartType:Vn})}},Un=class extends Nn{get[ee](){return Object.assign(super[ee],{menuPartType:$s,popupPartType:Yn,popupTogglePartType:zn,sourcePartType:Hn})}get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          [part~="menu"] {
            background: window;
            border: none;
            padding: 0.5em 0;
          }
        </style>
      `),e}};function Gn(e){return class extends e{constructor(){super();!this[Ee]&&this.attachInternals&&(this[Ee]=this.attachInternals())}attributeChangedCallback(e,t,s){if("current"===e){const t=w(e,s);this.current!==t&&(this.current=t)}else super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{current:!1})}[Ce](e){if(super[Ce](e),e.current){const{current:e}=this[Fe];C(this,"current",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.current){const{current:e}=this[Fe],t=new CustomEvent("current-changed",{bubbles:!0,detail:{current:e}});this.dispatchEvent(t);const s=new CustomEvent("currentchange",{bubbles:!0,detail:{current:e}});this.dispatchEvent(s)}}get current(){return this[Fe].current}set current(e){this[Me]({current:e})}}}customElements.define("elix-menu-button",class extends Un{});class qn extends(Gn(_s(Ut(G)))){}const Kn=qn,Zn=class extends Kn{get[Je](){return D.html`
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
    `}};customElements.define("elix-menu-item",class extends Zn{});const $n=class extends G{get disabled(){return!0}[Ce](e){super[Ce](e),this[re]&&this.setAttribute("aria-hidden","true")}},Jn=class extends $n{get[Je](){return D.html`
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
    `}};customElements.define("elix-menu-separator",class extends Jn{});const Qn=class extends On{get[ee](){return Object.assign(super[ee],{popupPartType:Yn,sourcePartType:Hn})}};customElements.define("elix-popup-button",class extends Qn{}),customElements.define("elix-popup",class extends Yn{});const Xn=Symbol("previousBodyStyleOverflow"),_n=Symbol("previousDocumentMarginRight"),er=Symbol("wrap"),tr=Symbol("wrappingFocus");function sr(e){return class extends e{[Te](e){const t=T(this[Re]);if(t){const s=document.activeElement&&(document.activeElement===t||document.activeElement.contains(t)),n=this[Re].activeElement,r=n&&(n===t||x(n,t));(s||r)&&"Tab"===e.key&&e.shiftKey&&(this[tr]=!0,this[we].focusCatcher.focus(),this[tr]=!1)}return super[Te]&&super[Te](e)||!1}[Ce](e){super[Ce]&&super[Ce](e),this[re]&&this[we].focusCatcher.addEventListener("focus",(()=>{if(!this[tr]){const e=T(this[Re]);e&&e.focus()}}))}[er](e){const t=A`
        <style>
          #focusCapture {
            display: flex;
            height: 100%;
            overflow: hidden;
            width: 100%;
          }

          #focusCaptureContainer {
            align-items: center;
            display: flex;
            flex: 1;
            flex-direction: column;
            justify-content: center;
            position: relative;
          }
        </style>
        <div id="focusCapture">
          <div id="focusCaptureContainer"></div>
          <div id="focusCatcher" tabindex="0"></div>
        </div>
      `,s=t.getElementById("focusCaptureContainer");s&&(e.replaceWith(t),s.append(e))}}}sr.wrap=er;const nr=sr,rr=class extends on{constructor(){super(),"PointerEvent"in window||this.addEventListener("touchmove",(e=>{1===e.touches.length&&e.preventDefault()}))}},or=function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:"dialog"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super[Te]&&super[Te](e)||!1}[Ce](e){if(super[Ce]&&super[Ce](e),e.opened)if(this[Fe].opened&&document.documentElement){const e=document.documentElement.clientWidth,t=window.innerWidth-e;this[Xn]=document.body.style.overflow,this[_n]=t>0?document.documentElement.style.marginRight:null,document.body.style.overflow="hidden",t>0&&(document.documentElement.style.marginRight=t+"px")}else null!=this[Xn]&&(document.body.style.overflow=this[Xn],this[Xn]=null),null!=this[_n]&&(document.documentElement.style.marginRight=this[_n],this[_n]=null);if(e.role){const{role:e}=this[Fe];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}(nr(os(gn))),ir=class extends or{get[ee](){return Object.assign(super[ee],{backdropPartType:rr,tabIndex:-1})}get[Je](){const e=super[Je],t=e.content.querySelector("#frame");return this[nr.wrap](t),e.content.append(A`
        <style>
          :host {
            height: 100%;
            left: 0;
            pointer-events: initial;
            top: 0;
            width: 100%;
          }
        </style>
      `),e}},ar=class extends rr{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: rgba(0, 0, 0, 0.2);
          }
        </style>
      `),e}};class lr extends(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{backdropPartType:ar,framePartType:Vn})}}}(ir)){}const cr=lr;function ur(e){return class extends e{get[ee](){return Object.assign(super[ee],{selectedText:""})}[ie](e){return super[ie]?super[ie](e):Ms(e)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,r=t?t[n]:null,o=r?this[ie](r):"";Object.assign(s,{selectedText:o})}return s}get selectedText(){return this[Fe].selectedText}set selectedText(e){const{items:t}=this[Fe],s=t?function(e,t,s){return e.findIndex((e=>t(e)===s))}(t,this[ie],e):-1;this[Me]({selectedIndex:s})}}}function dr(e){return class extends e{get[ee](){return Object.assign(super[ee],{value:null})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,r=t?t[n]:null,o=r?r.getAttribute("value"):"";Object.assign(s,{value:o})}return s}get value(){return this[Fe].value}set value(e){const{items:t}=this[Fe],s=t?function(e,t){return e.findIndex((e=>e.getAttribute("value")===t))}(t,e):-1;this[Me]({selectedIndex:s})}}}function hr(e){return class extends e{attributeChangedCallback(e,t,s){"selected-index"===e?this.selectedIndex=Number(s):super.attributeChangedCallback(e,t,s)}[Oe](e){if(super[Oe]&&super[Oe](e),e.selectedIndex&&this[ke]){const e=this[Fe].selectedIndex,t=new CustomEvent("selected-index-changed",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedindexchange",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(s)}}get selectedIndex(){const{items:e,selectedIndex:t}=this[Fe];return e&&e.length>0?t:-1}set selectedIndex(e){isNaN(e)||this[Me]({selectedIndex:e})}get selectedItem(){const{items:e,selectedIndex:t}=this[Fe];return e&&e[t]}set selectedItem(e){const{items:t}=this[Fe];if(!t)return;const s=t.indexOf(e);s>=0&&this[Me]({selectedIndex:s})}}}customElements.define("elix-dialog",class extends cr{});const pr=Is(tt(ns(Ls(Os(Dn(ur(dr(hr(Gs(On))))))))));function mr(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&$(n,s)}if(!s||s.valuePartType){const{valuePartType:s}=t,n=e.getElementById("value");n&&$(n,s)}}const gr=class extends pr{[J](e,t){L(t,(e?[...e.childNodes]:[]).map((e=>e.cloneNode(!0))))}get[ee](){return Object.assign(super[ee],{accessibleOptions:null,ariaHasPopup:"listbox",listPartType:"div",selectedIndex:-1,selectedItem:null,valuePartType:"div"})}get items(){const e=this[we]&&this[we].list;return e?e.items:null}get listPartType(){return this[Fe].listPartType}set listPartType(e){this[Me]({listPartType:e})}[Ce](e){if(super[Ce](e),mr(this[Re],this[Fe],e),e.items||e.selectedIndex){const{items:e,selectedIndex:t}=this[Fe],s=e?e[t]:null;this[J](s,this[we].value),e&&e.forEach((e=>{"selected"in e&&(e.selected=e===s)}))}if(e.sourcePartType){const e=this[we].source;e.inner&&e.inner.setAttribute("role","none")}}[Oe](e){super[Oe](e),e.listPartType&&this[Me]({popupList:this[we].list})}[Ne](e,t){const s=super[Ne](e,t);if(t.items){const t=(e.items||[]).map((e=>{const t=document.createElement("option");return t.textContent=e.textContent,t}));Object.assign(s,{accessibleOptions:t})}if(t.opened&&e.opened&&Object.assign(s,{currentIndex:e.selectedIndex}),t.opened){const{closeResult:n,currentIndex:r,opened:o}=e,i=t.opened&&!o,a=n&&n.canceled;i&&!a&&r>=0&&Object.assign(s,{selectedIndex:r})}if(t.items||t.selectedIndex){const{items:t,opened:n,selectedIndex:r}=e;!n&&r<0&&t&&t.length>0&&Object.assign(s,{selectedIndex:0})}return s}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="source"]');t&&Z(t,A` <div id="value" part="value"></div> `);const s=e.content.querySelector("slot:not([name])");s&&s.replaceWith(A`
        <div id="list" part="list">
          <slot></slot>
        </div>
      `);const n=e.content.querySelector('[part~="source"]');return n&&(n.setAttribute("aria-activedescendant","value"),n.setAttribute("aria-autocomplete","none"),n.setAttribute("aria-controls","list"),n.setAttribute("role","combobox")),mr(e.content,this[Fe]),e.content.append(A`
      <style>
        [part~="list"] {
          max-height: 100%;
        }
      </style>
    `),e}get valuePartType(){return this[Fe].valuePartType}set valuePartType(e){this[Me]({valuePartType:e})}},br=nn(et(Is(Cs(Ss(it(ns(Ls(Os(Rs(rs(os(js(zs(is(hr(ur(dr(Gs(qs(G)))))))))))))))))))),fr=class extends br{get[ee](){return Object.assign(super[ee],{highlightCurrentItem:!0,orientation:"vertical",role:"listbox"})}async flashCurrentItem(){const e=this[Fe].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Me]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Me]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[Ce](e){if(super[Ce](e),e.items||e.currentIndex||e.highlightCurrentItem){const{currentIndex:e,items:t,highlightCurrentItem:s}=this[Fe];t&&t.forEach(((t,n)=>{const r=n===e;t.toggleAttribute("current",s&&r),t.setAttribute("aria-selected",String(r))}))}}get[De](){return this[we].container}get[Je](){const e=super[Je];return e.content.append(A`
      <style>
        :host {
          box-sizing: border-box;
          cursor: default;
          display: flex;
          overflow: hidden; /* Container element is responsible for scrolling */
          -webkit-tap-highlight-color: transparent;
        }

        #container {
          display: "block";
          flex: 1;
          -webkit-overflow-scrolling: touch; /* for momentum scrolling */
          overflow-x: "hidden";
          overflow-y: "auto";
        }
      </style>
      <div id="container" role="none">
        <slot id="slot"></slot>
      </div>
    `),e}},yr=class extends fr{get[Je](){const e=super[Je];return e.content.append(A`
      <style>
        ::slotted(*),
        #slot > * {
          padding: 0.25em;
        }

        ::slotted([current]),
        #slot > [current] {
          background: highlight;
          color: highlighttext;
        }

        @media (pointer: coarse) {
          ::slotted(*),
          #slot > * {
            padding: 1em;
          }
        }
      </style>
    `),e}},wr=class extends gr{get[ee](){return Object.assign(super[ee],{listPartType:yr,popupPartType:Yn,sourcePartType:Hn,popupTogglePartType:zn})}};customElements.define("elix-dropdown-list",class extends wr{});class vr extends(nn(Gn(_s(Ut(G))))){get[ee](){return Object.assign(super[ee],{role:"option"})}get[Je](){return D.html`
      <style>
        :host {
          display: block;
        }
      </style>
      <slot></slot>
    `}}const xr=vr,Tr=class extends xr{get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <svg id="checkmark" xmlns="http://www.w3.org/2000/svg" viewBox="4 6 18 12">
          <path d="M0 0h24v24H0V0z" fill="none"/>
          <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41L9 16.17z"/>
        </svg>
        <slot></slot>
      `),e.content.append(A`
      <style>
        :host {
          white-space: nowrap;
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

    `),e}};customElements.define("elix-option",class extends Tr{})})();