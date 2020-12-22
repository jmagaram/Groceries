(()=>{"use strict";const e=Symbol("defaultState"),t=Symbol("delegatesFocus"),s=Symbol("firstRender"),n=Symbol("focusTarget"),i=Symbol("hasDynamicTemplate"),r=Symbol("ids"),o=Symbol("nativeInternals"),a=Symbol("raiseChangeEvents"),l=Symbol("render"),c=Symbol("renderChanges"),u=Symbol("rendered"),d=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),g=Symbol("shadowRootMode"),m=Symbol("state"),f=Symbol("stateEffects"),b=Symbol("template"),y=Symbol("mousedownListener");function w(e,t){return"boolean"==typeof t?t:"string"==typeof t&&(""===t||e.toLowerCase()===t.toLowerCase())}function x(e){for(const t of C(e)){const e=t[n]||t,s=e;if(e instanceof HTMLElement&&e.tabIndex>=0&&!s.disabled&&!(e instanceof HTMLSlotElement))return e}return null}function v(e,t){let s=t;for(;s;){const t=s.assignedSlot||s.parentNode||s.host;if(t===e)return!0;s=t}return!1}function T(e){const t=O(e,(e=>e instanceof HTMLElement&&e.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&e.tabIndex>=0)),{value:s}=t.next();return s instanceof HTMLElement?s:null}function E(e,t){e[y]&&e.removeEventListener("mousedown",e[y]),t&&(e[y]=e=>{if(0!==e.button)return;const s=x(t[n]||t);s&&(s.focus(),e.preventDefault())},e.addEventListener("mousedown",e[y]))}function P(e,t){return Array.prototype.findIndex.call(e,(e=>e===t||v(e,t)))}function I(e,t){const s=t.composedPath()[0];return e===s||v(e,s)}function*C(e){e&&(yield e,yield*function*(e){let t=e;for(;t=t instanceof HTMLElement&&t.assignedSlot?t.assignedSlot:t instanceof ShadowRoot?t.host:t.parentNode,t;)yield t}(e))}function S(e,t,s){e.toggleAttribute(t,s),e[o]&&e[o].states&&e[o].states.toggle(t,s)}const k={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function L(e,t){const s=[...t],n=e.childNodes.length,i=s.length,r=Math.max(n,i);for(let t=0;t<r;t++){const r=e.childNodes[t],o=s[t];t>=n?e.append(o):t>=i?e.removeChild(e.childNodes[i]):r!==o&&(s.indexOf(r,t)>=t?e.insertBefore(o,r):e.replaceChild(o,r))}}function*O(e,t){let s;if(t(e)&&(yield e),e instanceof HTMLElement&&e.shadowRoot)s=e.shadowRoot.children;else{const t=e instanceof HTMLSlotElement?e.assignedNodes({flatten:!0}):[];s=t.length>0?t:e.childNodes}if(s)for(let e=0;e<s.length;e++)yield*O(s[e],t)}const A=(e,...t)=>D.html(e,...t).content,D={html(e,...t){const s=document.createElement("template");return s.innerHTML=String.raw(e,...t),s}},j={tabindex:"tabIndex"},F={tabIndex:"tabindex"};function M(e){if(e===HTMLElement)return[];const t=Object.getPrototypeOf(e.prototype).constructor;let s=t.observedAttributes;s||(s=M(t));const n=Object.getOwnPropertyNames(e.prototype).filter((t=>{const s=Object.getOwnPropertyDescriptor(e.prototype,t);return s&&"function"==typeof s.set})).map((e=>function(e){let t=F[e];if(!t){const s=/([A-Z])/g;t=e.replace(s,"-$1").toLowerCase(),F[e]=t}return t}(e))).filter((e=>s.indexOf(e)<0));return s.concat(n)}const R=Symbol("state"),B=Symbol("raiseChangeEventsInNextRender"),W=Symbol("changedSinceLastRender");function H(e,t){const s={};for(const r in t)n=t[r],i=e[r],(n instanceof Date&&i instanceof Date?n.getTime()===i.getTime():n===i)||(s[r]=!0);var n,i;return s}const N=new Map,z=Symbol("shadowIdProxy"),Y=Symbol("proxyElement"),U={get(e,t){const s=e[Y][p];return s&&"string"==typeof t?s.getElementById(t):null}};function V(e){let t=e[i]?void 0:N.get(e.constructor);if(void 0===t){if(t=e[b],t&&!(t instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${e.constructor.name} must return an HTMLTemplateElement.`;e[i]||N.set(e.constructor,t||null)}return t}const $=function(e){return class extends e{attributeChangedCallback(e,t,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(e,t,s),s!==t&&!this[d]){const t=function(e){let t=j[e];if(!t){const s=/-([a-z])/g;t=e.replace(s,(e=>e[1].toUpperCase())),j[e]=t}return t}(e);if(t in this){const n=k[e]?w(e,s):s;this[t]=n}}}static get observedAttributes(){return M(this)}}}(function(t){class n extends t{constructor(){super(),this[s]=void 0,this[a]=!1,this[W]=null,this[h](this[e])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[c]()}get[e](){return super[e]||{}}[l](e){super[l]&&super[l](e)}[c](){void 0===this[s]&&(this[s]=!0);const e=this[W];if(this[s]||e){const t=this[a];this[a]=this[B],this[d]=!0,this[l](e),this[d]=!1,this[W]=null,this[u](e),this[s]=!1,this[a]=t,this[B]=t}}[u](e){super[u]&&super[u](e)}async[h](e){this[d]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:t,changed:n}=function(e,t){const s=Object.assign({},e[R]),n={};let i=t;for(;;){const t=H(s,i);if(0===Object.keys(t).length)break;Object.assign(s,i),Object.assign(n,t),i=e[f](s,t)}return{state:s,changed:n}}(this,e);if(this[R]&&0===Object.keys(n).length)return;Object.freeze(t),this[R]=t,this[a]&&(this[B]=!0);const i=void 0===this[s]||null!==this[W];this[W]=Object.assign(this[W]||{},n),this.isConnected&&!i&&(await Promise.resolve(),this[c]())}get[m](){return this[R]}[f](e,t){return super[f]?super[f](e,t):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(n.prototype,"state",{get(){return this[m]}}),n}(function(e){return class extends e{get[r](){if(!this[z]){const e={[Y]:this};this[z]=new Proxy(e,U)}return this[z]}[l](e){if(super[l]&&super[l](e),!this[p]){const e=V(this);if(e){const s=this.attachShadow({delegatesFocus:this[t],mode:this[g]}),n=document.importNode(e.content,!0);s.append(n),this[p]=s}else this[p]=null}}get[g](){return"open"}}}(HTMLElement))),q=new Map;function G(e){if("function"==typeof e){let t;try{t=new e}catch(s){if("TypeError"!==s.name)throw s;!function(e){let t;const s=e.name&&e.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const e=/([A-Z])/g;t=s[0].replace(e,((e,t,s)=>s>0?"-"+t:t)).toLowerCase()}else t="custom-element";let n,i=q.get(t)||0;for(;n=`${t}-${i}`,customElements.get(n);i++);customElements.define(n,e),q.set(t,i+1)}(e),t=new e}return t}return document.createElement(e)}function K(e,t){const s=e.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";return(e instanceof HTMLElement||e instanceof SVGElement)&&(t instanceof HTMLElement||t instanceof SVGElement)&&(Array.prototype.forEach.call(e.attributes,(e=>{t.getAttribute(e.name)||"class"===e.name||"style"===e.name||t.setAttribute(e.name,e.value)})),Array.prototype.forEach.call(e.classList,(e=>{t.classList.add(e)})),Array.prototype.forEach.call(e.style,(s=>{t.style[s]||(t.style[s]=e.style[s])}))),t.append(...e.childNodes),s.replaceChild(t,e),t}function X(e,t){if("function"==typeof t&&e.constructor===t||"string"==typeof t&&e instanceof Element&&e.localName===t)return e;{const s=G(t);return K(e,s),s}}const Z=Symbol("applyElementData"),J=Symbol("checkSize"),Q=Symbol("closestAvailableItemIndex"),_=Symbol("contentSlot"),ee=e,te=Symbol("defaultTabIndex"),se=t,ne=Symbol("effectEndTarget"),ie=s,re=n,oe=Symbol("getItemText"),ae=Symbol("goDown"),le=Symbol("goEnd"),ce=Symbol("goFirst"),ue=Symbol("goLast"),de=Symbol("goLeft"),he=Symbol("goNext"),pe=Symbol("goPrevious"),ge=Symbol("goRight"),me=Symbol("goStart"),fe=Symbol("goToItemWithPrefix"),be=Symbol("goUp"),ye=i,we=r,xe=Symbol("inputDelegate"),ve=Symbol("itemsDelegate"),Te=Symbol("keydown"),Ee=Symbol("matchText"),Pe=Symbol("mouseenter"),Ie=Symbol("mouseleave"),Ce=o,Se=a,ke=l,Le=c,Oe=Symbol("renderDataToElement"),Ae=u,De=d,je=Symbol("scrollTarget"),Fe=h,Me=p,Re=g,Be=Symbol("startEffect"),We=m,He=f,Ne=Symbol("swipeDown"),ze=Symbol("swipeDownComplete"),Ye=Symbol("swipeLeft"),Ue=Symbol("swipeLeftTransitionEnd"),Ve=Symbol("swipeRight"),$e=Symbol("swipeRightTransitionEnd"),qe=Symbol("swipeUp"),Ge=Symbol("swipeUpComplete"),Ke=Symbol("swipeStart"),Xe=Symbol("swipeTarget"),Ze=Symbol("tap"),Je=b,Qe=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:J,closestAvailableItemIndex:Q,contentSlot:_,defaultState:ee,defaultTabIndex:te,delegatesFocus:se,effectEndTarget:ne,firstRender:ie,focusTarget:re,getItemText:oe,goDown:ae,goEnd:le,goFirst:ce,goLast:ue,goLeft:de,goNext:he,goPrevious:pe,goRight:ge,goStart:me,goToItemWithPrefix:fe,goUp:be,hasDynamicTemplate:ye,ids:we,inputDelegate:xe,itemsDelegate:ve,keydown:Te,mouseenter:Pe,mouseleave:Ie,nativeInternals:Ce,event,raiseChangeEvents:Se,render:ke,renderChanges:Le,renderDataToElement:Oe,rendered:Ae,rendering:De,scrollTarget:je,setState:Fe,shadowRoot:Me,shadowRootMode:Re,startEffect:Be,state:We,stateEffects:He,swipeDown:Ne,swipeDownComplete:ze,swipeLeft:Ye,swipeLeftTransitionEnd:Ue,swipeRight:Ve,swipeRightTransitionEnd:$e,swipeUp:qe,swipeUpComplete:Ge,swipeStart:Ke,swipeTarget:Xe,tap:Ze,template:Je,toggleSelectedFlag:Qe}});const _e=document.createElement("div");_e.attachShadow({mode:"open",delegatesFocus:!0});const et=_e.shadowRoot.delegatesFocus;function tt(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{composeFocus:!et})}[ke](e){super[ke]&&super[ke](e),this[ie]&&this.addEventListener("mousedown",(e=>{if(this[We].composeFocus&&0===e.button&&e.target instanceof Element){const t=x(e.target);t&&(t.focus(),e.preventDefault())}}))}}}function st(e){return class extends e{get ariaLabel(){return this[We].ariaLabel}set ariaLabel(e){this[We].removingAriaAttribute||this[Fe]({ariaLabel:String(e)})}get ariaLabelledby(){return this[We].ariaLabelledby}set ariaLabelledby(e){this[We].removingAriaAttribute||this[Fe]({ariaLabelledby:String(e)})}get[ee](){return Object.assign(super[ee]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[ke](e){if(super[ke]&&super[ke](e),this[ie]&&this.addEventListener("focus",(()=>{this[Se]=!0;const e=it(this,this[We]);this[Fe]({inputLabel:e}),this[Se]=!1})),e.inputLabel){const{inputLabel:e}=this[We];e?this[xe].setAttribute("aria-label",e):this[xe].removeAttribute("aria-label")}}[Ae](e){super[Ae]&&super[Ae](e),this[ie]&&(window.requestIdleCallback||setTimeout)((()=>{const e=it(this,this[We]);this[Fe]({inputLabel:e})}));const{ariaLabel:t,ariaLabelledby:s}=this[We];e.ariaLabel&&!this[We].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",t),this[Fe]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),e.ariaLabelledby&&!this[We].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Fe]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),e.removingAriaAttribute&&this[We].removingAriaAttribute&&this[Fe]({removingAriaAttribute:!1})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.ariaLabel&&e.ariaLabel||t.selectedText&&e.ariaLabelledby&&this.matches(":focus-within")){const t=it(this,e);Object.assign(s,{inputLabel:t})}return s}}}function nt(e){if("selectedText"in e)return e.selectedText;if("value"in e&&"options"in e){const t=e.value,s=e.options.find((e=>e.value===t));return s?s.innerText:""}return"value"in e?e.value:e.innerText}function it(e,t){const{ariaLabel:s,ariaLabelledby:n}=t,i=e.isConnected?e.getRootNode():null;let r=null;if(n&&i)r=n.split(" ").map((s=>{const n=i.getElementById(s);return n?n===e&&null!==t.value?t.selectedText:nt(n):""})).join(" ");else if(s)r=s;else if(i){const t=e.id;if(t){const e=i.querySelector(`[for="${t}"]`);e instanceof HTMLElement&&(r=nt(e))}if(null===r){const t=e.closest("label");t&&(r=nt(t))}}return r&&(r=r.trim()),r}let rt=!1;const ot=Symbol("focusVisibleChangedListener");function at(e){return class extends e{constructor(){super(),this.addEventListener("focusout",(e=>{Promise.resolve().then((()=>{const t=e.relatedTarget||document.activeElement,s=this===t,n=v(this,t);!s&&!n&&(this[Fe]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[ot]),this[ot]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[We].focusVisible!==rt&&this[Fe]({focusVisible:rt}),this[ot]||(this[ot]=()=>{this[Fe]({focusVisible:rt})},document.addEventListener("focusvisiblechange",this[ot]))}))}))}get[ee](){return Object.assign(super[ee]||{},{focusVisible:!1})}[ke](e){if(super[ke]&&super[ke](e),e.focusVisible){const{focusVisible:e}=this[We];this.toggleAttribute("focus-visible",e)}}get[Je](){const e=super[Je]||D.html``;return e.content.append(A`
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
      `),e}}}function lt(e){if(rt!==e){rt=e;const t=new CustomEvent("focus-visible-changed",{detail:{focusVisible:rt}});document.dispatchEvent(t);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:rt}});document.dispatchEvent(s)}}function ct(e){return class extends e{get[se](){return!0}focus(e){const t=this[re];t&&t.focus(e)}get[re](){return T(this[Me])}}}window.addEventListener("keydown",(()=>{lt(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{lt(!1)}),{capture:!0});const ut=Symbol("extends"),dt=Symbol("delegatedPropertySetters"),ht={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},pt={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},gt=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],mt={abort:!0,change:!0,reset:!0},ft=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],bt=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],yt=ct($);class wt extends yt{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if(bt.indexOf(e)>=0){const t=Object.assign({},this[We].innerAttributes,{[e]:s});this[Fe]({innerAttributes:t})}else super.attributeChangedCallback(e,t,s)}blur(){this.inner.blur()}get[ee](){return Object.assign(super[ee],{innerAttributes:{}})}get[te](){return ht[this.extends]?0:-1}get extends(){return this.constructor[ut]}get inner(){const e=this[we]&&this[we].inner;return e||console.warn("Attempted to get an inner standard element before it was instantiated."),e}static get observedAttributes(){return[...super.observedAttributes,...bt]}[ke](e){super[ke](e);const t=this.inner;if(this[ie]&&((pt[this.extends]||[]).forEach((e=>{t.addEventListener(e,(()=>{const t=new Event(e,{bubbles:mt[e]||!1});this.dispatchEvent(t)}))})),"disabled"in t&&gt.forEach((e=>{this.addEventListener(e,(e=>{t.disabled&&e.stopImmediatePropagation()}))}))),e.tabIndex&&(t.tabIndex=this[We].tabIndex),e.innerAttributes){const{innerAttributes:e}=this[We];for(const s in e)xt(t,s,e[s])}this.constructor[dt].forEach((s=>{if(e[s]){const e=this[We][s];("selectionEnd"===s||"selectionStart"===s)&&null===e||(t[s]=e)}}))}[Ae](e){if(super[Ae](e),e.disabled){const{disabled:e}=this[We];void 0!==e&&S(this,"disabled",e)}}get[Je](){const e=ft.includes(this.extends)?"block":"inline-block",t=this.extends;return D.html`
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
      <${t} id="inner" part="inner ${t}">
        <slot></slot>
      </${t}>
    `}static wrap(e){class t extends wt{}t[ut]=e;const s=document.createElement(e);return function(e,t){const s=Object.getOwnPropertyNames(t);e[dt]=[],s.forEach((s=>{const n=Object.getOwnPropertyDescriptor(t,s);if(!n)return;const i=function(e,t){if("function"==typeof t.value){if("constructor"!==e)return function(e,t){return{configurable:t.configurable,enumerable:t.enumerable,value:function(...t){this.inner[e](...t)},writable:t.writable}}(e,t)}else if("function"==typeof t.get||"function"==typeof t.set)return function(e,t){const s={configurable:t.configurable,enumerable:t.enumerable};return t.get&&(s.get=function(){return function(e,t){return e[We][t]||e[Me]&&e.inner[t]}(this,e)}),t.set&&(s.set=function(t){!function(e,t,s){e[We][t]!==s&&e[Fe]({[t]:s})}(this,e,t)}),t.writable&&(s.writable=t.writable),s}(e,t);return null}(s,n);i&&(Object.defineProperty(e.prototype,s,i),i.set&&e[dt].push(s))}))}(t,Object.getPrototypeOf(s)),t}}function xt(e,t,s){k[t]?"string"==typeof s?e.setAttribute(t,""):null===s&&e.removeAttribute(t):null!=s?e.setAttribute(t,s.toString()):e.removeAttribute(t)}const vt=wt,Tt=tt(st(at(vt.wrap("button")))),Et=class extends Tt{get[ee](){return Object.assign(super[ee],{role:"button"})}get[xe](){return this[we].inner}[Ze](){const e=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(e)}get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            display: inline-flex;
            outline: none;
            -webkit-tap-highlight-color: transparent;
            touch-action: manipulation;
          }

          [part~="button"] {
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
      `),e}},Pt=Symbol("wrap");function It(e){return class extends e{get arrowButtonOverlap(){return this[We].arrowButtonOverlap}set arrowButtonOverlap(e){this[Fe]({arrowButtonOverlap:e})}get arrowButtonPartType(){return this[We].arrowButtonPartType}set arrowButtonPartType(e){this[Fe]({arrowButtonPartType:e})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[pe]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[he]()}attributeChangedCallback(e,t,s){"arrow-button-overlap"===e?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===e?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{arrowButtonOverlap:!0,arrowButtonPartType:Et,orientation:"horizontal",showArrowButtons:!0})}[ke](e){if(e.arrowButtonPartType){const e=this[we].arrowButtonPrevious;e instanceof HTMLElement&&E(e,null);const t=this[we].arrowButtonNext;t instanceof HTMLElement&&E(t,null)}if(super[ke]&&super[ke](e),St(this[Me],this[We],e),e.arrowButtonPartType){const e=this,t=this[we].arrowButtonPrevious;t instanceof HTMLElement&&E(t,e);const s=Ct(this,(()=>this.arrowButtonPrevious()));t.addEventListener("mousedown",s);const n=this[we].arrowButtonNext;n instanceof HTMLElement&&E(n,e);const i=Ct(this,(()=>this.arrowButtonNext()));n.addEventListener("mousedown",i)}const{arrowButtonOverlap:t,canGoNext:s,canGoPrevious:n,orientation:i,rightToLeft:r}=this[We],o="vertical"===i,a=this[we].arrowButtonPrevious,l=this[we].arrowButtonNext;if(e.arrowButtonOverlap||e.orientation||e.rightToLeft){this[we].arrowDirection.style.flexDirection=o?"column":"row";const e={bottom:null,left:null,right:null,top:null};let s,n;t?Object.assign(e,{position:"absolute","z-index":1}):Object.assign(e,{position:null,"z-index":null}),t&&(o?(Object.assign(e,{left:0,right:0}),s={top:0},n={bottom:0}):(Object.assign(e,{bottom:0,top:0}),r?(s={right:0},n={left:0}):(s={left:0},n={right:0}))),Object.assign(a.style,e,s),Object.assign(l.style,e,n)}if(e.canGoNext&&null!==s&&(l.disabled=!s),e.canGoPrevious&&null!==n&&(a.disabled=!n),e.showArrowButtons){const e=this[We].showArrowButtons?null:"none";a.style.display=e,l.style.display=e}}get showArrowButtons(){return this[We].showArrowButtons}set showArrowButtons(e){this[Fe]({showArrowButtons:e})}[Pt](e){const t=A`
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
      `;St(t,this[We]);const s=t.getElementById("arrowDirectionContainer");s&&(e.replaceWith(t),s.append(e))}}}function Ct(e,t){return async function(s){0===s.button&&(e[Se]=!0,t()&&s.stopPropagation(),await Promise.resolve(),e[Se]=!1)}}function St(e,t,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=t,n=e.getElementById("arrowButtonPrevious");n&&X(n,s);const i=e.getElementById("arrowButtonNext");i&&X(i,s)}}It.wrap=Pt;const kt=It,Lt={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},Ot=864e5;function At(e,t){const s=e.includes("-ca-")?"":"-ca-gregory",n=e.includes("-nu-")?"":"-nu-latn",i=`${e}${s||n?"-u":""}${s}${n}`;return new Intl.DateTimeFormat(i,t)}function Dt(e,t){return null===e&&null===t||null!==e&&null!==t&&e.getTime()===t.getTime()}function jt(e,t){const s=Ft(t);return(e.getDay()-s+7)%7}function Ft(e){const t=Vt(e),s=Lt.firstDay[t];return void 0!==s?s:Lt.firstDay["001"]}function Mt(e){const t=Rt(e);return t.setDate(1),t}function Rt(e){const t=new Date(e.getTime());return t.setHours(0),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Bt(e){const t=new Date(e.getTime());return t.setHours(12),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Wt(e,t){const s=Bt(e);return s.setDate(s.getDate()+t),Ut(e,s),s}function Ht(e,t){const s=Bt(e);return s.setMonth(e.getMonth()+t),Ut(e,s),s}function Nt(){return Rt(new Date)}function zt(e){const t=Vt(e),s=Lt.weekendEnd[t];return void 0!==s?s:Lt.weekendEnd["001"]}function Yt(e){const t=Vt(e),s=Lt.weekendStart[t];return void 0!==s?s:Lt.weekendStart["001"]}function Ut(e,t){t.setHours(e.getHours()),t.setMinutes(e.getMinutes()),t.setSeconds(e.getSeconds()),t.setMilliseconds(e.getMilliseconds())}function Vt(e){const t=e?e.split("-"):null;return t?t[1]:"001"}function $t(e){return class extends e{attributeChangedCallback(e,t,s){"date"===e?this.date=new Date(s):super.attributeChangedCallback(e,t,s)}get date(){return this[We].date}set date(e){Dt(e,this[We].date)||this[Fe]({date:e})}get[ee](){return Object.assign(super[ee]||{},{date:null,locale:navigator.language})}get locale(){return this[We].locale}set locale(e){this[Fe]({locale:String(e)})}[Ae](e){if(super[Ae]&&super[Ae](e),e.date&&this[Se]){const e=this[We].date,t=new CustomEvent("date-changed",{bubbles:!0,detail:{date:e}});this.dispatchEvent(t);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:e}});this.dispatchEvent(s)}}}}function qt(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}get[ee](){return Object.assign(super[ee]||{},{selected:!1})}[ke](e){if(super[ke](e),e.selected){const{selected:e}=this[We];S(this,"selected",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.selected){const{selected:e}=this[We],t=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(s)}}get selected(){return this[We].selected}set selected(e){this[Fe]({selected:e})}}}const Gt=$t(qt($)),Kt=class extends Gt{get[ee](){return Object.assign(super[ee],{date:Nt(),outsideRange:!1})}[ke](e){super[ke](e);const{date:t}=this[We];if(e.date){const e=Nt(),s=t.getDay(),n=t.getDate(),i=Wt(t,1),r=Math.round(t.getTime()-e.getTime())/Ot;S(this,"alternate-month",Math.abs(t.getMonth()-e.getMonth())%2==1),S(this,"first-day-of-month",1===n),S(this,"first-week",n<=7),S(this,"future",t>e),S(this,"last-day-of-month",t.getMonth()!==i.getMonth()),S(this,"past",t<e),S(this,"sunday",0===s),S(this,"monday",1===s),S(this,"tuesday",2===s),S(this,"wednesday",3===s),S(this,"thursday",4===s),S(this,"friday",5===s),S(this,"saturday",6===s),S(this,"today",0===r),this[we].day.textContent=n.toString()}if(e.date||e.locale){const e=t.getDay(),{locale:s}=this[We],n=e===Yt(s)||e===zt(s);S(this,"weekday",!n),S(this,"weekend",n)}e.outsideRange&&S(this,"outside-range",this[We].outsideRange)}get outsideRange(){return this[We].outsideRange}set outsideRange(e){this[Fe]({outsideRange:e})}get[Je](){return D.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Xt=qt(Et),Zt=$t(class extends Xt{}),Jt=class extends Zt{get[ee](){return Object.assign(super[ee],{date:Nt(),dayPartType:Kt,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[We].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get outsideRange(){return this[We].outsideRange}set outsideRange(e){this[Fe]({outsideRange:e})}[ke](e){if(super[ke](e),e.dayPartType){const{dayPartType:e}=this[We];X(this[we].day,e)}const t=this[we].day;(e.dayPartType||e.date)&&(t.date=this[We].date),(e.dayPartType||e.locale)&&(t.locale=this[We].locale),(e.dayPartType||e.outsideRange)&&(t.outsideRange=this[We].outsideRange),(e.dayPartType||e.selected)&&(t.selected=this[We].selected)}get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");if(t){const e=G(this[We].dayPartType);e.id="day",t.replaceWith(e)}return e.content.append(A`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),e}},Qt=class extends ${get[ee](){return Object.assign(super[ee],{format:"short",locale:navigator.language})}get format(){return this[We].format}set format(e){this[Fe]({format:e})}get locale(){return this[We].locale}set locale(e){this[Fe]({locale:String(e)})}[ke](e){if(super[ke](e),e.format||e.locale){const{format:e,locale:t}=this[We],s=At(t,{weekday:e}),n=Ft(t),i=Yt(t),r=zt(t),o=new Date(2017,0,1),a=this[Me].querySelectorAll('[part~="day-name"]');for(let e=0;e<=6;e++){const t=(n+e)%7;o.setDate(t+1);const l=t===i||t===r,c=a[e];c.toggleAttribute("weekday",!l),c.toggleAttribute("weekend",l),c.textContent=s.format(o)}}}get[Je](){return D.html`
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
    `}},_t=$t($),es=class extends _t{attributeChangedCallback(e,t,s){"start-date"===e?this.startDate=new Date(s):super.attributeChangedCallback(e,t,s)}dayElementForDate(e){return(this.days||[]).find((t=>Dt(t.date,e)))}get dayCount(){return this[We].dayCount}set dayCount(e){this[Fe]({dayCount:e})}get dayPartType(){return this[We].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get days(){return this[We].days}get[ee](){const e=Nt();return Object.assign(super[ee],{date:e,dayCount:1,dayPartType:Kt,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:e})}[ke](e){if(super[ke](e),e.days&&L(this[we].dayContainer,this[We].days),e.date||e.locale||e.showSelectedDay){const e=this[We].showSelectedDay,{date:t}=this[We],s=t.getDate(),n=t.getMonth(),i=t.getFullYear();(this.days||[]).forEach((t=>{const r=t.date,o=e&&r.getDate()===s&&r.getMonth()===n&&r.getFullYear()===i;t.toggleAttribute("selected",o)}))}if(e.dayCount||e.startDate){const{dayCount:e,startDate:t}=this[We],s=Wt(t,e);(this[We].days||[]).forEach((e=>{if("outsideRange"in e){const n=e.date.getTime(),i=n<t.getTime()||n>=s.getTime();e.outsideRange=i}}))}}get showCompleteWeeks(){return this[We].showCompleteWeeks}set showCompleteWeeks(e){this[Fe]({showCompleteWeeks:e})}get showSelectedDay(){return this[We].showSelectedDay}set showSelectedDay(e){this[Fe]({showSelectedDay:e})}get startDate(){return this[We].startDate}set startDate(e){Dt(this[We].startDate,e)||this[Fe]({startDate:e})}[He](e,t){const s=super[He](e,t);if(t.dayCount||t.dayPartType||t.locale||t.showCompleteWeeks||t.startDate){const n=function(e,t){const{dayCount:s,dayPartType:n,locale:i,showCompleteWeeks:r,startDate:o}=e,a=r?function(e,t){return Rt(Wt(e,-jt(e,t)))}(o,i):Rt(o);let l;if(r){c=a,u=function(e,t){return Rt(Wt(e,6-jt(e,t)))}(Wt(o,s-1),i),l=Math.round((u.getTime()-c.getTime())/Ot)+1}else l=s;var c,u;let d=e.days?e.days.slice():[],h=a;for(let e=0;e<l;e++){const s=t||e>=d.length,r=s?G(n):d[e];r.date=new Date(h.getTime()),r.locale=i,"part"in r&&(r.part="day"),r.style.gridColumnStart="",s&&(d[e]=r),h=Wt(h,1)}l<d.length&&(d=d.slice(0,l));const p=d[0];if(p&&!r){const t=jt(p.date,e.locale);p.style.gridColumnStart=t+1}return Object.freeze(d),d}(e,t.dayPartType);Object.assign(s,{days:n})}return s}get[Je](){return D.html`
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
    `}},ts=$t($),ss=class extends ts{get[ee](){return Object.assign(super[ee],{date:Nt(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[We].monthFormat}set monthFormat(e){this[Fe]({monthFormat:e})}[ke](e){if(super[ke](e),e.date||e.locale||e.monthFormat||e.yearFormat){const{date:e,locale:t,monthFormat:s,yearFormat:n}=this[We],i={};s&&(i.month=s),n&&(i.year=n);const r=At(t,i);this[we].formatted.textContent=r.format(e)}}get[Je](){return D.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[We].yearFormat}set yearFormat(e){this[Fe]({yearFormat:e})}},ns=$t($);function is(e,t,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=t,n=e.getElementById("dayNamesHeader");n&&X(n,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=t,n=e.getElementById("monthYearHeader");n&&X(n,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=t,n=e.getElementById("monthDays");n&&X(n,s)}}function rs(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}checkValidity(){return this[Ce].checkValidity()}get[ee](){return Object.assign(super[ee]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[Ce]}static get formAssociated(){return!0}get form(){return this[Ce].form}get name(){return this[We]?this[We].name:""}set name(t){const s=String(t);"name"in e.prototype&&(super.name=s),this[Fe]({name:s})}[ke](e){if(super[ke]&&super[ke](e),e.name){const{name:e}=this[We];e?this.setAttribute("name",e):this.removeAttribute("name")}if(this[Ce]&&this[Ce].setValidity&&(e.valid||e.validationMessage)){const{valid:e,validationMessage:t}=this[We];e?this[Ce].setValidity({}):this[Ce].setValidity({customError:!0},t)}}[Ae](e){super[Ae]&&super[Ae](e),e.value&&this[Ce]&&this[Ce].setFormValue(this[We].value,this[We])}reportValidity(){return this[Ce].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[We].validationMessage}get validity(){return this[Ce].validity}get willValidate(){return this[Ce].willValidate}}}function os(e){return class extends e{[ae](){if(super[ae])return super[ae]()}[le](){if(super[le])return super[le]()}[de](){if(super[de])return super[de]()}[ge](){if(super[ge])return super[ge]()}[me](){if(super[me])return super[me]()}[be](){if(super[be])return super[be]()}[Te](e){let t=!1;const s=this[We].orientation||"both",n="horizontal"===s||"both"===s,i="vertical"===s||"both"===s;switch(e.key){case"ArrowDown":i&&(t=e.altKey?this[le]():this[ae]());break;case"ArrowLeft":!n||e.metaKey||e.altKey||(t=this[de]());break;case"ArrowRight":!n||e.metaKey||e.altKey||(t=this[ge]());break;case"ArrowUp":i&&(t=e.altKey?this[me]():this[be]());break;case"End":t=this[le]();break;case"Home":t=this[me]()}return t||super[Te]&&super[Te](e)||!1}}}function as(e){return class extends e{constructor(){super(),this.addEventListener("keydown",(async e=>{this[Se]=!0,this[We].focusVisible||this[Fe]({focusVisible:!0}),this[Te](e)&&(e.preventDefault(),e.stopImmediatePropagation()),await Promise.resolve(),this[Se]=!1}))}attributeChangedCallback(e,t,s){if("tabindex"===e){let e;null===s?e=-1:(e=Number(s),isNaN(e)&&(e=this[te]?this[te]:0)),this.tabIndex=e}else super.attributeChangedCallback(e,t,s)}get[ee](){const e=this[se]?-1:0;return Object.assign(super[ee]||{},{tabIndex:e})}[Te](e){return!!super[Te]&&super[Te](e)}[ke](e){super[ke]&&super[ke](e),e.tabIndex&&(this.tabIndex=this[We].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(e){super.tabIndex!==e&&(super.tabIndex=e),this[De]||this[Fe]({tabIndex:e})}}}function ls(e){return class extends e{connectedCallback(){const e="rtl"===getComputedStyle(this).direction;this[Fe]({rightToLeft:e}),super.connectedCallback()}}}const cs=kt($t(at(rs(os(as(ls(class extends ns{dayElementForDate(e){const t=this[we].monthDays;return t&&"dayElementForDate"in t&&t.dayElementForDate(e)}get dayNamesHeaderPartType(){return this[We].dayNamesHeaderPartType}set dayNamesHeaderPartType(e){this[Fe]({dayNamesHeaderPartType:e})}get dayPartType(){return this[We].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get days(){return this[Me]?this[we].monthDays.days:[]}get daysOfWeekFormat(){return this[We].daysOfWeekFormat}set daysOfWeekFormat(e){this[Fe]({daysOfWeekFormat:e})}get[ee](){return Object.assign(super[ee],{date:Nt(),dayNamesHeaderPartType:Qt,dayPartType:Kt,daysOfWeekFormat:"short",monthDaysPartType:es,monthFormat:"long",monthYearHeaderPartType:ss,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[We].monthFormat}set monthFormat(e){this[Fe]({monthFormat:e})}get monthDaysPartType(){return this[We].monthDaysPartType}set monthDaysPartType(e){this[Fe]({monthDaysPartType:e})}get monthYearHeaderPartType(){return this[We].monthYearHeaderPartType}set monthYearHeaderPartType(e){this[Fe]({monthYearHeaderPartType:e})}[ke](e){if(super[ke](e),is(this[Me],this[We],e),(e.dayPartType||e.monthDaysPartType)&&(this[we].monthDays.dayPartType=this[We].dayPartType),e.locale||e.monthDaysPartType||e.monthYearHeaderPartType||e.dayNamesHeaderPartType){const e=this[We].locale;this[we].monthDays.locale=e,this[we].monthYearHeader.locale=e,this[we].dayNamesHeader.locale=e}if(e.date||e.monthDaysPartType){const{date:e}=this[We];if(e){const t=Mt(e),s=function(e){const t=Mt(e);return t.setMonth(t.getMonth()+1),t.setDate(t.getDate()-1),t}(e).getDate();Object.assign(this[we].monthDays,{date:e,dayCount:s,startDate:t}),this[we].monthYearHeader.date=Mt(e)}}if(e.daysOfWeekFormat||e.dayNamesHeaderPartType){const{daysOfWeekFormat:e}=this[We];this[we].dayNamesHeader.format=e}if(e.showCompleteWeeks||e.monthDaysPartType){const{showCompleteWeeks:e}=this[We];this[we].monthDays.showCompleteWeeks=e}if(e.showSelectedDay||e.monthDaysPartType){const{showSelectedDay:e}=this[We];this[we].monthDays.showSelectedDay=e}if(e.monthFormat||e.monthYearHeaderPartType){const{monthFormat:e}=this[We];this[we].monthYearHeader.monthFormat=e}if(e.yearFormat||e.monthYearHeaderPartType){const{yearFormat:e}=this[We];this[we].monthYearHeader.yearFormat=e}}get showCompleteWeeks(){return this[We].showCompleteWeeks}set showCompleteWeeks(e){this[Fe]({showCompleteWeeks:e})}get showSelectedDay(){return this[We].showSelectedDay}set showSelectedDay(e){this[Fe]({showSelectedDay:e})}get[Je](){const e=D.html`
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
    `;return is(e.content,this[We]),e}get yearFormat(){return this[We].yearFormat}set yearFormat(e){this[Fe]({yearFormat:e})}}))))))),us=class extends cs{constructor(){super(),this.addEventListener("mousedown",(e=>{if(0!==e.button)return;this[Se]=!0;const t=e.composedPath()[0];if(t instanceof Node){const e=this.days,s=e[P(e,t)];s&&(this.date=s.date)}this[Se]=!1})),E(this,this)}arrowButtonNext(){const e=this[We].date||Nt();return this[Fe]({date:Ht(e,1)}),!0}arrowButtonPrevious(){const e=this[We].date||Nt();return this[Fe]({date:Ht(e,-1)}),!0}get[ee](){return Object.assign(super[ee],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Nt(),dayPartType:Jt,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[Te](e){let t=!1;switch(e.key){case"Home":this[Fe]({date:Nt()}),t=!0;break;case"PageDown":this[Fe]({date:Ht(this[We].date,1)}),t=!0;break;case"PageUp":this[Fe]({date:Ht(this[We].date,-1)}),t=!0}return t||super[Te]&&super[Te](e)}[ae](){return super[ae]&&super[ae](),this[Fe]({date:Wt(this[We].date,7)}),!0}[de](){return super[de]&&super[de](),this[Fe]({date:Wt(this[We].date,-1)}),!0}[ge](){return super[ge]&&super[ge](),this[Fe]({date:Wt(this[We].date,1)}),!0}[be](){return super[be]&&super[be](),this[Fe]({date:Wt(this[We].date,-7)}),!0}[He](e,t){const s=super[He](e,t);return t.date&&Object.assign(s,{value:e.date?e.date.toString():""}),s}get[Je](){const e=super[Je],t=e.content.querySelector("#monthYearHeader");this[kt.wrap](t);const s=D.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return e.content.append(s.content),e}get value(){return this.date}set value(e){this.date=e}},ds=new Set;function hs(e){return class extends e{attributeChangedCallback(e,t,s){if("dark"===e){const t=w(e,s);this.dark!==t&&(this.dark=t)}else super.attributeChangedCallback(e,t,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),ds.delete(this)}get dark(){return this[We].dark}set dark(e){this[Fe]({dark:e})}get[ee](){return Object.assign(super[ee]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[We].detectDarkMode}set detectDarkMode(e){"auto"!==e&&"off"!==e||this[Fe]({detectDarkMode:e})}[ke](e){if(super[ke]&&super[ke](e),e.dark){const{dark:e}=this[We];S(this,"dark",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.detectDarkMode){const{detectDarkMode:e}=this[We];"auto"===e?(ds.add(this),ps(this)):ds.delete(this)}}}}function ps(e){const t=function(e){const t=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(e);return t?{r:t[1],g:t[2],b:t[3]}:null}(gs(e));if(t){const s=function(e){const t=e.r/255,s=e.g/255,n=e.b/255,i=Math.max(t,s,n),r=Math.min(t,s,n);let o=0,a=0,l=(i+r)/2;const c=i-r;if(0!==c){switch(a=l>.5?c/(2-c):c/(i+r),i){case t:o=(s-n)/c+(s<n?6:0);break;case s:o=(n-t)/c+2;break;case n:o=(t-s)/c+4}o/=6}return{h:o,s:a,l}}(t).l<.5;e[Fe]({dark:s})}}function gs(e){const t="rgb(255,255,255)";if(e instanceof Document)return t;const s=getComputedStyle(e).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(e.assignedSlot)return gs(e.assignedSlot);const n=e.parentNode;return n instanceof ShadowRoot?gs(n.host):n instanceof Element?gs(n):t}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{ds.forEach((e=>{ps(e)}))}));class ms extends(function(e){return class extends e{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host([disabled]) ::slotted(*) {
            opacity: 0.5;
          }

          [part~="button"] {
            display: inline-flex;
            justify-content: center;
            margin: 0;
            position: relative;
          }
        </style>
      `),e}}}(Et)){}const fs=ms,bs=hs(fs),ys=class extends bs{get[Je](){const e=super[Je];return e.content.append(A`
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

          [part~="button"] {
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
      `),e}},ws=class extends Kt{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},xs=class extends Jt{get[ee](){return Object.assign(super[ee],{dayPartType:ws})}get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},vs=class extends Qt{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},Ts=class extends ss{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),e}};class Es extends(hs(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{arrowButtonPartType:ys})}[ke](e){if(super[ke](e),e.orientation||e.rightToLeft){const{orientation:e,rightToLeft:t}=this[We],s="vertical"===e?"rotate(90deg)":t?"rotateZ(180deg)":"";this[we].arrowIconPrevious&&(this[we].arrowIconPrevious.style.transform=s),this[we].arrowIconNext&&(this[we].arrowIconNext.style.transform=s)}if(e.dark){const{dark:e}=this[We],t=this[we].arrowButtonPrevious,s=this[we].arrowButtonNext;"dark"in t&&(t.dark=e),"dark"in s&&(s.dark=e)}}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="arrowButtonPrevious"]');t&&t.append(A`
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
          `),e}}}(us))){get[ee](){return Object.assign(super[ee],{dayNamesHeaderPartType:vs,dayPartType:xs,monthYearHeaderPartType:Ts})}}const Ps=Es;customElements.define("elix-calendar-month-navigator",class extends Ps{});const Is=Symbol("generatedId");let Cs=0;const Ss={a:"link",article:"region",button:"button",h1:"sectionhead",h2:"sectionhead",h3:"sectionhead",h4:"sectionhead",h5:"sectionhead",h6:"sectionhead",hr:"sectionhead",iframe:"region",link:"link",menu:"menu",ol:"list",option:"option",output:"liveregion",progress:"progressbar",select:"select",table:"table",td:"td",textarea:"textbox",th:"th",ul:"list"};function ks(e){let t=e.id||e[Is];return t||(t="_id"+Cs++,e[Is]=t),t}function Ls(e){return class extends e{attributeChangedCallback(e,t,s){if("current-index"===e)this.currentIndex=Number(s);else if("current-item-required"===e){const t=w(e,s);this.currentItemRequired!==t&&(this.currentItemRequired=t)}else if("cursor-operations-wrap"===e){const t=w(e,s);this.cursorOperationsWrap!==t&&(this.cursorOperationsWrap=t)}else super.attributeChangedCallback(e,t,s)}get currentIndex(){const{items:e,currentIndex:t}=this[We];return e&&e.length>0?t:-1}set currentIndex(e){isNaN(e)||this[Fe]({currentIndex:e})}get currentItem(){const{items:e,currentIndex:t}=this[We];return e&&e[t]}set currentItem(e){const{items:t}=this[We];if(!t)return;const s=t.indexOf(e);s>=0&&this[Fe]({currentIndex:s})}get currentItemRequired(){return this[We].currentItemRequired}set currentItemRequired(e){this[Fe]({currentItemRequired:e})}get cursorOperationsWrap(){return this[We].cursorOperationsWrap}set cursorOperationsWrap(e){this[Fe]({cursorOperationsWrap:e})}goFirst(){return super.goFirst&&super.goFirst(),this[ce]()}goLast(){return super.goLast&&super.goLast(),this[ue]()}goNext(){return super.goNext&&super.goNext(),this[he]()}goPrevious(){return super.goPrevious&&super.goPrevious(),this[pe]()}[Ae](e){if(super[Ae]&&super[Ae](e),e.currentIndex&&this[Se]){const{currentIndex:e}=this[We],t=new CustomEvent("current-index-changed",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("currentindexchange",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(s)}}}}function Os(e,t,s){if(!(e instanceof Node))return!1;for(const n of C(e))if(n instanceof HTMLElement){const e=getComputedStyle(n),i="vertical"===t;if(i&&("scroll"===e.overflowY||"auto"===e.overflowY)||!i&&("scroll"===e.overflowX||"auto"===e.overflowX)){const e=i?"scrollTop":"scrollLeft";if(!s&&n[e]>0)return!0;const t=i?"clientHeight":"clientWidth",r=n[i?"scrollHeight":"scrollWidth"]-n[t];if(s&&n[e]<r)return!0}}return!1}function As(e){const t=e[Me],s=t&&t.querySelector("slot:not([name])");return s&&s.parentNode instanceof Element&&function(e){for(const t of C(e))if(t instanceof HTMLElement&&Ds(t))return t;return null}(s.parentNode)||e}function Ds(e){const t=getComputedStyle(e),s=t.overflowX,n=t.overflowY;return"scroll"===s||"auto"===s||"scroll"===n||"auto"===n}function js(e){return class extends e{[Ae](e){super[Ae]&&super[Ae](e),e.currentItem&&this.scrollCurrentItemIntoView()}scrollCurrentItemIntoView(){super.scrollCurrentItemIntoView&&super.scrollCurrentItemIntoView();const{currentItem:e,items:t}=this[We];if(!e||!t)return;const s=this[je].getBoundingClientRect(),n=e.getBoundingClientRect(),i=n.bottom-s.bottom,r=n.left-s.left,o=n.right-s.right,a=n.top-s.top,l=this[We].orientation||"both";"horizontal"!==l&&"both"!==l||(o>0?this[je].scrollLeft+=o:r<0&&(this[je].scrollLeft+=Math.ceil(r))),"vertical"!==l&&"both"!==l||(i>0?this[je].scrollTop+=i:a<0&&(this[je].scrollTop+=Math.ceil(a)))}get[je](){return super[je]||As(this)}}}function Fs(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{canGoDown:null,canGoLeft:null,canGoRight:null,canGoUp:null})}[ae](){return super[ae]&&super[ae](),this[he]()}[le](){return super[le]&&super[le](),this[ue]()}[de](){return super[de]&&super[de](),this[We]&&this[We].rightToLeft?this[he]():this[pe]()}[ge](){return super[ge]&&super[ge](),this[We]&&this[We].rightToLeft?this[pe]():this[he]()}[me](){return super[me]&&super[me](),this[ce]()}[be](){return super[be]&&super[be](),this[pe]()}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.canGoNext||t.canGoPrevious||t.languageDirection||t.orientation||t.rightToLeft){const{canGoNext:t,canGoPrevious:n,orientation:i,rightToLeft:r}=e,o="horizontal"===i||"both"===i,a="vertical"===i||"both"===i,l=a&&t,c=!!o&&(r?t:n),u=!!o&&(r?n:t),d=a&&n;Object.assign(s,{canGoDown:l,canGoLeft:c,canGoRight:u,canGoUp:d})}return s}}}function Ms(e){return class extends e{get items(){return this[We]?this[We].items:null}[Ae](e){if(super[Ae]&&super[Ae](e),!this[ie]&&e.items&&this[Se]){const e=new CustomEvent("items-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("itemschange",{bubbles:!0});this.dispatchEvent(t)}}}}function Rs(e){return class extends e{[Q](e,t={}){const s=void 0!==t.direction?t.direction:1,n=void 0!==t.index?t.index:e.currentIndex,i=void 0!==t.wrap?t.wrap:e.cursorOperationsWrap,{items:r}=e,o=r?r.length:0;if(0===o)return-1;if(i){let t=(n%o+o)%o;const i=((t-s)%o+o)%o;for(;t!==i;){if(!e.availableItemFlags||e.availableItemFlags[t])return t;t=((t+s)%o+o)%o}}else for(let t=n;t>=0&&t<o;t+=s)if(!e.availableItemFlags||e.availableItemFlags[t])return t;return-1}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,desiredCurrentIndex:null,currentItem:null,currentItemRequired:!1,cursorOperationsWrap:!1})}[ce](){return super[ce]&&super[ce](),Bs(this,0,1)}[ue](){return super[ue]&&super[ue](),Bs(this,this[We].items.length-1,-1)}[he](){super[he]&&super[he]();const{currentIndex:e,items:t}=this[We];return Bs(this,e<0&&t?0:e+1,1)}[pe](){super[pe]&&super[pe]();const{currentIndex:e,items:t}=this[We];return Bs(this,e<0&&t?t.length-1:e-1,-1)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.availableItemFlags||t.items||t.currentIndex||t.currentItemRequired){const{currentIndex:n,desiredCurrentIndex:i,currentItem:r,currentItemRequired:o,items:a}=e,l=a?a.length:0;let c,u=i;if(t.items&&!t.currentIndex&&r&&l>0&&a[n]!==r){const e=a.indexOf(r);e>=0&&(u=e)}else t.currentIndex&&(n<0&&null!==r||n>=0&&(0===l||a[n]!==r)||null===i)&&(u=n);o&&u<0&&(u=0),u<0?(u=-1,c=-1):0===l?c=-1:(c=Math.max(Math.min(l-1,u),0),c=this[Q](e,{direction:1,index:c,wrap:!1}),c<0&&(c=this[Q](e,{direction:-1,index:c-1,wrap:!1})));const d=a&&a[c]||null;Object.assign(s,{currentIndex:c,desiredCurrentIndex:u,currentItem:d})}return s}}}function Bs(e,t,s){const n=e[Q](e[We],{direction:s,index:t});if(n<0)return!1;const i=e[We].currentIndex!==n;return i&&e[Fe]({currentIndex:n}),i}const Ws=["applet","basefont","embed","font","frame","frameset","isindex","keygen","link","multicol","nextid","noscript","object","param","script","style","template","noembed"];function Hs(e){return e.getAttribute("aria-label")||e.getAttribute("alt")||e.innerText||e.textContent||""}function Ns(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{texts:null})}[oe](e){return super[oe]?super[oe](e):Hs(e)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items){const{items:t}=e,n=function(e,t){return e?Array.from(e,(e=>t(e))):null}(t,this[oe]);n&&(Object.freeze(n),Object.assign(s,{texts:n}))}return s}}}function zs(e){return class extends e{[Te](e){let t=!1;if("horizontal"!==this.orientation)switch(e.key){case"PageDown":t=this.pageDown();break;case"PageUp":t=this.pageUp()}return t||super[Te]&&super[Te](e)}get orientation(){return super.orientation||this[We]&&this[We].orientation||"both"}pageDown(){return super.pageDown&&super.pageDown(),Us(this,!0)}pageUp(){return super.pageUp&&super.pageUp(),Us(this,!1)}get[je](){return super[je]||As(this)}}}function Ys(e,t,s){const n=e[We].items,i=s?0:n.length-1,r=s?n.length:0,o=s?1:-1;let a,l,c=null;const{availableItemFlags:u}=e[We];for(a=i;a!==r;a+=o)if((!u||u[a])&&(l=n[a].getBoundingClientRect(),l.top<=t&&t<=l.bottom)){c=n[a];break}if(!c||!l)return null;const d=getComputedStyle(c),h=d.paddingTop?parseFloat(d.paddingTop):0,p=d.paddingBottom?parseFloat(d.paddingBottom):0,g=l.top+h,m=g+c.clientHeight-h-p;return s&&g<=t||!s&&m>=t?a:a-o}function Us(e,t){const s=e[We].items,n=e[We].currentIndex,i=e[je].getBoundingClientRect(),r=Ys(e,t?i.bottom:i.top,t);let o;if(r&&n===r){const i=s[n].getBoundingClientRect(),r=e[je].clientHeight;o=Ys(e,t?i.bottom+r:i.top-r,t)}else o=r;if(!o){const n=t?s.length-1:0;o=e[Q]?e[Q](e[We],{direction:t?-1:1,index:n}):n}const a=o!==n;if(a){const t=e[Se];e[Se]=!0,e[Fe]({currentIndex:o}),e[Se]=t}return a}const Vs=Symbol("typedPrefix"),$s=Symbol("prefixTimeout");function qs(e){return class extends e{constructor(){super(),Ks(this)}[fe](e){if(super[fe]&&super[fe](e),null==e||0===e.length)return!1;const t=e.toLowerCase(),s=this[We].texts.findIndex((s=>s.substr(0,e.length).toLowerCase()===t));if(s>=0){const e=this[We].currentIndex;return this[Fe]({currentIndex:s}),this[We].currentIndex!==e}return!1}[Te](e){let t;switch(e.key){case"Backspace":!function(e){const t=e,s=t[Vs]?t[Vs].length:0;s>0&&(t[Vs]=t[Vs].substr(0,s-1)),e[fe](t[Vs]),Xs(e)}(this),t=!0;break;case"Escape":Ks(this);break;default:e.ctrlKey||e.metaKey||e.altKey||1!==e.key.length||function(e,t){const s=e,n=s[Vs]||"";s[Vs]=n+t,e[fe](s[Vs]),Xs(e)}(this,e.key)}return t||super[Te]&&super[Te](e)}}}function Gs(e){const t=e;t[$s]&&(clearTimeout(t[$s]),t[$s]=!1)}function Ks(e){e[Vs]="",Gs(e)}function Xs(e){Gs(e),e[$s]=setTimeout((()=>{Ks(e)}),1e3)}function Zs(e){return class extends e{get[_](){const e=this[Me]&&this[Me].querySelector("slot:not([name])");return this[Me]&&e||console.warn(`SlotContentMixin expects ${this.constructor.name} to define a shadow tree that includes a default (unnamed) slot.\nSee https://elix.org/documentation/SlotContentMixin.`),e}get[ee](){return Object.assign(super[ee]||{},{content:null})}[Ae](e){if(super[Ae]&&super[Ae](e),this[ie]){const e=this[_];e&&e.addEventListener("slotchange",(async()=>{this[Se]=!0;const t=e.assignedNodes({flatten:!0});Object.freeze(t),this[Fe]({content:t}),await Promise.resolve(),this[Se]=!1}))}}}}function Js(e){return function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{items:null})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.content){const t=e.content,n=t?Array.prototype.filter.call(t,(e=>{return(t=e)instanceof Element&&(!t.localName||Ws.indexOf(t.localName)<0);var t})):null;n&&Object.freeze(n),Object.assign(s,{items:n})}return s}}}(Zs(e))}function Qs(e){return class extends e{constructor(){super(),this.addEventListener("mousedown",(e=>{0===e.button&&(this[Se]=!0,this[Ze](e),this[Se]=!1)}))}[ke](e){super[ke]&&super[ke](e),this[ie]&&Object.assign(this.style,{touchAction:"manipulation",mozUserSelect:"none",msUserSelect:"none",webkitUserSelect:"none",userSelect:"none"})}[Ze](e){const t=e.composedPath?e.composedPath()[0]:e.target,{items:s,currentItemRequired:n}=this[We];if(s&&t instanceof Node){const i=P(s,t),r=i>=0?s[i]:null;(r&&!r.disabled||!r&&!n)&&(this[Fe]({currentIndex:i}),e.stopPropagation())}}}}const _s=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"menuitem",role:e.role||"menu"})}get itemRole(){return this[We].itemRole}set itemRole(e){this[Fe]({itemRole:e})}[ke](e){super[ke]&&super[ke](e);const t=this[We].items;if((e.items||e.itemRole)&&t){const{itemRole:e}=this[We];t.forEach((t=>{e===Ss[t.localName]?t.removeAttribute("role"):t.setAttribute("role",e)}))}if(e.role){const{role:e}=this[We];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(Ls(js(ct(Fs(Ms(Rs(Ns(os(as(zs(qs(ls(Js(Qs($))))))))))))))),en=class extends _s{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,highlightCurrentItem:!0,orientation:"vertical",currentItemFocused:!1})}async flashCurrentItem(){const e=this[We].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Fe]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Fe]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[ke](e){super[ke](e),this[ie]&&(this.addEventListener("disabledchange",(e=>{this[Se]=!0;const t=e.target,{items:s}=this[We],n=null===s?-1:s.indexOf(t);if(n>=0){const e=this[We].availableItemFlags.slice();e[n]=!t.disabled,this[Fe]({availableItemFlags:e})}this[Se]=!1})),"PointerEvent"in window?this.addEventListener("pointerdown",(e=>this[Ze](e))):this.addEventListener("touchstart",(e=>this[Ze](e))),this.removeAttribute("tabindex"));const{currentIndex:t,items:s}=this[We];if((e.items||e.currentIndex||e.highlightCurrentItem)&&s){const{highlightCurrentItem:e}=this[We];s.forEach(((s,n)=>{s.toggleAttribute("current",e&&n===t)}))}(e.items||e.currentIndex||e.currentItemFocused||e.focusVisible)&&s&&s.forEach(((e,s)=>{const n=s===t,i=t<0&&0===s;this[We].currentItemFocused?n||i||e.removeAttribute("tabindex"):(n||i)&&(e.tabIndex=0)}))}[Ae](e){if(super[Ae](e),!this[ie]&&e.currentIndex&&!this[We].currentItemFocused){const{currentItem:e}=this[We];(e instanceof HTMLElement?e:this).focus(),this[Fe]({currentItemFocused:!0})}}get[je](){return this[we].content}[He](e,t){const s=super[He](e,t);if(t.currentIndex&&Object.assign(s,{currentItemFocused:!1}),t.items){const{items:t}=e,n=null===t?null:t.map((e=>!e.disabled));Object.assign(s,{availableItemFlags:n})}return s}get[Je](){return D.html`
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
    `}},tn=class extends en{get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}};customElements.define("elix-menu",class extends tn{});const sn=Symbol("documentMouseupListener");function nn(e){return class extends e{connectedCallback(){super.connectedCallback(),on(this)}get[ee](){return Object.assign(super[ee]||{},{dragSelect:!0})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),on(this)}[Ae](e){super[Ae](e),e.opened&&on(this)}[He](e,t){const s=super[He](e,t);return t.opened&&e.opened&&Object.assign(s,{dragSelect:!0}),s}}}async function rn(e){const t=this,s=t[Me].elementsFromPoint(e.clientX,e.clientY);if(t.opened){const e=s.indexOf(t[we].source)>=0,n=t[we].popup,i=s.indexOf(n)>=0,r=n.frame&&s.indexOf(n.frame)>=0;e?t[We].dragSelect&&(t[Se]=!0,t[Fe]({dragSelect:!1}),t[Se]=!1):i||r||(t[Se]=!0,await t.close(),t[Se]=!1)}}function on(e){e[We].opened&&e.isConnected?e[sn]||(e[sn]=rn.bind(e),document.addEventListener("mouseup",e[sn])):e[sn]&&(document.removeEventListener("mouseup",e[sn]),e[sn]=null)}function an(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{disabled:!1})}get disabled(){return this[We].disabled}set disabled(e){this[Fe]({disabled:e})}[Ae](e){if(super[Ae]&&super[Ae](e),e.disabled&&(this.toggleAttribute("disabled",this.disabled),this[Se])){const e=new CustomEvent("disabled-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("disabledchange",{bubbles:!0});this.dispatchEvent(t)}}}}const ln=Symbol("closePromise"),cn=Symbol("closeResolve");function un(e){return class extends e{attributeChangedCallback(e,t,s){if("opened"===e){const t=w(e,s);this.opened!==t&&(this.opened=t)}else super.attributeChangedCallback(e,t,s)}async close(e){super.close&&await super.close(),this[Fe]({closeResult:e}),await this.toggle(!1)}get closed(){return this[We]&&!this[We].opened}get closeFinished(){return this[We].closeFinished}get closeResult(){return this[We].closeResult}get[ee](){const e={closeResult:null,opened:!1};return this[Be]&&Object.assign(e,{closeFinished:!0,effect:"close",effectPhase:"after",openCloseEffects:!0}),Object.assign(super[ee]||{},e)}async open(){super.open&&await super.open(),this[Fe]({closeResult:void 0}),await this.toggle(!0)}get opened(){return this[We]&&this[We].opened}set opened(e){this[Fe]({closeResult:void 0}),this.toggle(e)}[ke](e){if(super[ke](e),e.opened){const{opened:e}=this[We];S(this,"opened",e)}if(e.closeFinished){const{closeFinished:e}=this[We];S(this,"closed",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.opened&&this[Se]){const e=new CustomEvent("opened-changed",{bubbles:!0,detail:{closeResult:this[We].closeResult,opened:this[We].opened}});this.dispatchEvent(e);const t=new CustomEvent("openedchange",{bubbles:!0,detail:{closeResult:this[We].closeResult,opened:this[We].opened}});if(this.dispatchEvent(t),this[We].opened){const e=new CustomEvent("opened",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("open",{bubbles:!0});this.dispatchEvent(t)}else{const e=new CustomEvent("closed",{bubbles:!0,detail:{closeResult:this[We].closeResult}});this.dispatchEvent(e);const t=new CustomEvent("close",{bubbles:!0,detail:{closeResult:this[We].closeResult}});this.dispatchEvent(t)}}const t=this[cn];this.closeFinished&&t&&(this[cn]=null,this[ln]=null,t(this[We].closeResult))}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.openCloseEffects||t.effect||t.effectPhase||t.opened){const{effect:t,effectPhase:n,openCloseEffects:i,opened:r}=e,o=i?"close"===t&&"after"===n:!r;Object.assign(s,{closeFinished:o})}return s}async toggle(e=!this.opened){if(super.toggle&&await super.toggle(e),e!==this[We].opened){const t={opened:e};this[We].openCloseEffects&&(t.effect=e?"open":"close","after"===this[We].effectPhase&&(t.effectPhase="before")),await this[Fe](t)}}whenClosed(){return this[ln]||(this[ln]=new Promise((e=>{this[cn]=e}))),this[ln]}}}function dn(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:null})}[ke](e){if(super[ke]&&super[ke](e),e.role){const{role:e}=this[We];e?this.setAttribute("role",e):this.removeAttribute("role")}}get role(){return super.role}set role(e){const t=String(e);super.role=t,this[De]||this[Fe]({s:t})}}}const hn=dn($),pn=class extends hn{get[ee](){return Object.assign(super[ee],{role:"none"})}get[Je](){return D.html`
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
    `}},gn=class extends ${get[Je](){return D.html`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }
      </style>
      <slot></slot>
    `}},mn=Symbol("appendedToDocument"),fn=Symbol("assignedZIndex"),bn=Symbol("restoreFocusToElement");function yn(e){const t=function(){const e=document.body.querySelectorAll("*"),t=Array.from(e,(e=>{const t=getComputedStyle(e);let s=0;if("static"!==t.position&&"auto"!==t.zIndex){const e=t.zIndex?parseInt(t.zIndex):0;s=isNaN(e)?0:e}return s}));return Math.max(...t)}()+1;e[fn]=t,e.style.zIndex=t.toString()}function wn(e){const t=getComputedStyle(e).zIndex,s=e.style.zIndex,n=!isNaN(parseInt(s));if("auto"===t)return n;if("0"===t&&!n){const t=e.assignedSlot||(e instanceof ShadowRoot?e.host:e.parentNode);if(!(t instanceof HTMLElement))return!0;if(!wn(t))return!1}return!0}const xn=un(function(e){return class extends e{get autoFocus(){return this[We].autoFocus}set autoFocus(e){this[Fe]({autoFocus:e})}get[ee](){return Object.assign(super[ee]||{},{autoFocus:!0,persistent:!1})}async open(){this[We].persistent||this.isConnected||(this[mn]=!0,document.body.append(this)),super.open&&await super.open()}[ke](e){super[ke]&&super[ke](e),this[ie]&&this.addEventListener("blur",(e=>{const t=e.relatedTarget||document.activeElement;t instanceof HTMLElement&&(v(this,t)||(this.opened?this[bn]=t:(t.focus(),this[bn]=null)))})),(e.effectPhase||e.opened||e.persistent)&&!this[We].persistent&&((void 0===this.closeFinished?this.closed:this.closeFinished)?this[fn]&&(this.style.zIndex="",this[fn]=null):this[fn]?this.style.zIndex=this[fn]:wn(this)||yn(this))}[Ae](e){if(super[Ae]&&super[Ae](e),this[ie]&&this[We].persistent&&!wn(this)&&yn(this),e.opened&&this[We].autoFocus)if(this[We].opened){this[bn]||document.activeElement===document.body||(this[bn]=document.activeElement);const e=T(this);e&&e.focus()}else this[bn]&&(this[bn].focus(),this[bn]=null);!this[ie]&&!this[We].persistent&&this.closeFinished&&this[mn]&&(this[mn]=!1,this.parentNode&&this.parentNode.removeChild(this))}get[Je](){const e=super[Je]||D.html``;return e.content.append(A`
        <style>
          :host([closed]) {
            display: none;
          }
        </style>
      `),e}}}(Zs($)));function vn(e,t,s){if(!s||s.backdropPartType){const{backdropPartType:s}=t,n=e.getElementById("backdrop");n&&X(n,s)}if(!s||s.framePartType){const{framePartType:s}=t,n=e.getElementById("frame");n&&X(n,s)}}const Tn=class extends xn{get backdrop(){return this[we]&&this[we].backdrop}get backdropPartType(){return this[We].backdropPartType}set backdropPartType(e){this[Fe]({backdropPartType:e})}get[ee](){return Object.assign(super[ee],{backdropPartType:pn,framePartType:gn})}get frame(){return this[we].frame}get framePartType(){return this[We].framePartType}set framePartType(e){this[Fe]({framePartType:e})}[ke](e){super[ke](e),vn(this[Me],this[We],e)}[Ae](e){super[Ae](e),e.opened&&this[We].content&&this[We].content.forEach((e=>{e[J]&&e[J]()}))}get[Je](){const e=super[Je];return e.content.append(A`
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
    `),vn(e.content,this[We]),e}},En=Symbol("implicitCloseListener");async function Pn(e){const t=this,s=e.relatedTarget||document.activeElement;s instanceof Element&&!v(t,s)&&(t[Se]=!0,await t.close({canceled:"window blur"}),t[Se]=!1)}async function In(e){const t=this,s="resize"!==e.type||t[We].closeOnWindowResize;!I(t,e)&&s&&(t[Se]=!0,await t.close({canceled:"window "+e.type}),t[Se]=!1)}const Cn=as(function(e){return class extends e{constructor(){super(),this.addEventListener("blur",Pn.bind(this))}get closeOnWindowResize(){return this[We].closeOnWindowResize}set closeOnWindowResize(e){this[Fe]({closeOnWindowResize:e})}get[ee](){return Object.assign(super[ee]||{},{closeOnWindowResize:!0,role:"alert"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super.keydown&&super.keydown(e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.role){const{role:e}=this[We];this.setAttribute("role",e)}}[Ae](e){var t;super[Ae]&&super[Ae](e),e.opened&&(this.opened?("requestIdleCallback"in window?window.requestIdleCallback:setTimeout)((()=>{var e;this.opened&&((e=this)[En]=In.bind(e),window.addEventListener("blur",e[En]),window.addEventListener("resize",e[En]),window.addEventListener("scroll",e[En]))})):(t=this)[En]&&(window.removeEventListener("blur",t[En]),window.removeEventListener("resize",t[En]),window.removeEventListener("scroll",t[En]),t[En]=null))}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(Tn));async function Sn(e){const t=this;t[Se]=!0,await t.close({canceled:"mousedown outside"}),t[Se]=!1,e.preventDefault(),e.stopPropagation()}const kn=class extends Cn{[ke](e){super[ke](e),e.backdropPartType&&(this[we].backdrop.addEventListener("mousedown",Sn.bind(this)),"PointerEvent"in window||this[we].backdrop.addEventListener("touchend",Sn))}},Ln=Symbol("resizeListener"),On=an(at(ls(un($))));function An(e){const t=window.innerHeight,s=window.innerWidth,n=e[we].popup.getBoundingClientRect(),i=e.getBoundingClientRect(),r=n.height,o=n.width,{horizontalAlign:a,popupPosition:l,rightToLeft:c}=e[We],u=i.top,d=Math.ceil(t-i.bottom),h=i.right,p=Math.ceil(s-i.left),g=r<=u,m=r<=d,f="below"===l,b=f&&(m||d>=u)||!f&&!g&&d>=u,y=b&&m||!b&&g?null:b?d:u,w=b?"below":"above";let x,v,T;if("stretch"===a)x=0,v=0,T=null;else{const e="left"===a||(c?"end"===a:"start"===a),t=e&&(o<=p||p>=h)||!e&&!(o<=h)&&p>=h;x=t?0:null,v=t?null:0,T=t&&p||!t&&h?null:t?p:h}e[Fe]({calculatedFrameMaxHeight:y,calculatedFrameMaxWidth:T,calculatedPopupLeft:x,calculatedPopupPosition:w,calculatedPopupRight:v,popupMeasured:!0})}function Dn(e,t,s){if(!s||s.popupPartType){const{popupPartType:s}=t,n=e.getElementById("popup");n&&X(n,s)}if(!s||s.sourcePartType){const{sourcePartType:s}=t,n=e.getElementById("source");n&&X(n,s)}}const jn=class extends On{get[ee](){return Object.assign(super[ee],{ariaHasPopup:"true",horizontalAlign:"start",popupHeight:null,popupMeasured:!1,popupPosition:"below",popupPartType:kn,popupWidth:null,roomAbove:null,roomBelow:null,roomLeft:null,roomRight:null,sourcePartType:"div"})}get[xe](){return this[we].source}get frame(){return this[we].popup.frame}get horizontalAlign(){return this[We].horizontalAlign}set horizontalAlign(e){this[Fe]({horizontalAlign:e})}get popupPosition(){return this[We].popupPosition}set popupPosition(e){this[Fe]({popupPosition:e})}get popupPartType(){return this[We].popupPartType}set popupPartType(e){this[Fe]({popupPartType:e})}[ke](e){if(super[ke](e),Dn(this[Me],this[We],e),this[ie]||e.ariaHasPopup){const{ariaHasPopup:e}=this[We];null===e?this[xe].removeAttribute("aria-haspopup"):this[xe].setAttribute("aria-haspopup",this[We].ariaHasPopup)}if(e.popupPartType&&(this[we].popup.addEventListener("open",(()=>{this.opened||(this[Se]=!0,this.open(),this[Se]=!1)})),this[we].popup.addEventListener("close",(e=>{if(!this.closed){this[Se]=!0;const t=e.detail.closeResult;this.close(t),this[Se]=!1}}))),e.opened||e.popupMeasured){const{calculatedFrameMaxHeight:e,calculatedFrameMaxWidth:t,calculatedPopupLeft:s,calculatedPopupPosition:n,calculatedPopupRight:i,opened:r,popupMeasured:o}=this[We];if(r)if(o){const r="below"===n,o=this[we].popup;Object.assign(o.style,{bottom:r?"":0,left:s,opacity:"",right:i});const a=o.frame;Object.assign(a.style,{maxHeight:e?e+"px":"",maxWidth:t?t+"px":""}),Object.assign(this[we].popupContainer.style,{overflow:"",top:r?"":"0"})}else Object.assign(this[we].popupContainer.style,{overflow:"hidden"}),Object.assign(this[we].popup.style,{opacity:0});else r||(Object.assign(this[we].popupContainer.style,{overflow:""}),Object.assign(this[we].popup.style,{bottom:"",left:"",opacity:"",position:"",right:""}))}if(e.opened){const{opened:e}=this[We];this[we].popup.opened=e}if(e.disabled&&"disabled"in this[we].source){const{disabled:e}=this[We];this[we].source.disabled=e}if(e.calculatedPopupPosition){const{calculatedPopupPosition:e}=this[We],t=this[we].popup;"position"in t&&(t.position=e)}}[Ae](e){super[Ae](e);const{opened:t}=this[We];var s;e.opened?t?(s=this,setTimeout((()=>{s[We].opened&&(An(s),function(e){const t=e;t[Ln]=()=>{An(e)},window.addEventListener("resize",t[Ln])}(s))}))):function(e){const t=e;t[Ln]&&(window.removeEventListener("resize",t[Ln]),t[Ln]=null)}(this):t&&!this[We].popupMeasured&&An(this)}get sourcePartType(){return this[We].sourcePartType}set sourcePartType(e){this[Fe]({sourcePartType:e})}[He](e,t){const s=super[He](e,t);return(t.opened&&!e.opened||e.opened&&(t.horizontalAlign||t.rightToLeft))&&Object.assign(s,{calculatedFrameMaxHeight:null,calculatedFrameMaxWidth:null,calculatedPopupLeft:null,calculatedPopupPosition:null,calculatedPopupRight:null,popupMeasured:!1}),s}get[Je](){const e=super[Je];return e.content.append(A`
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
    `),Dn(e.content,this[We]),e}},Fn=ct(as(nn(jn))),Mn=class extends Fn{get[ee](){return Object.assign(super[ee],{sourcePartType:"button"})}[Te](e){let t;switch(e.key){case" ":case"ArrowDown":case"ArrowUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0);break;case"Escape":this.opened&&(this.close({canceled:"Escape"}),t=!0)}if(t=super[Te]&&super[Te](e),!t&&this.opened&&!e.metaKey&&!e.altKey)switch(e.key){case"ArrowDown":case"ArrowLeft":case"ArrowRight":case"ArrowUp":case"End":case"Home":case"PageDown":case"PageUp":case" ":t=!0}return t}[ke](e){if(super[ke](e),this[ie]&&this[we].source.addEventListener("focus",(async e=>{const t=I(this[we].popup,e),s=null!==this[We].popupHeight;!t&&this.opened&&s&&(this[Se]=!0,await this.close(),this[Se]=!1)})),e.opened){const{opened:e}=this[We];this.toggleAttribute("opened",e)}e.sourcePartType&&this[we].source.addEventListener("mousedown",(e=>{if(this.disabled)return void e.preventDefault();const t=e;t.button&&0!==t.button||(setTimeout((()=>{this.opened||(this[Se]=!0,this.open(),this[Se]=!1)})),e.stopPropagation())})),e.popupPartType&&this[we].popup.removeAttribute("tabindex")}get[Je](){const e=super[Je];return e.content.append(A`
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
      `),e}},Rn=Symbol("documentMousemoveListener");function Bn(e){return class extends e{connectedCallback(){super.connectedCallback(),Hn(this)}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,hasHoveredOverItemSinceOpened:!1,popupList:null})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Hn(this)}[Te](e){let t=!1;switch(e.key){case"Enter":this.opened&&(Nn(this),t=!0)}return t||super[Te]&&super[Te](e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.popupList){const{popupList:e}=this[We];e&&(e.addEventListener("mouseup",(async e=>{const t=this[We].currentIndex;this[We].dragSelect||t>=0?(e.stopPropagation(),this[Se]=!0,await Nn(this),this[Se]=!1):e.stopPropagation()})),e.addEventListener("currentindexchange",(e=>{this[Se]=!0;const t=e;this[Fe]({currentIndex:t.detail.currentIndex}),this[Se]=!1})))}if(e.currentIndex||e.popupList){const{currentIndex:e,popupList:t}=this[We];t&&"currentIndex"in t&&(t.currentIndex=e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.opened){if(this[We].opened){const{popupList:e}=this[We];e.scrollCurrentItemIntoView&&setTimeout((()=>{e.scrollCurrentItemIntoView()}))}Hn(this)}}[He](e,t){const s=super[He]?super[He](e,t):{};return t.opened&&e.opened&&Object.assign(s,{hasHoveredOverItemSinceOpened:!1}),s}}}function Wn(e){const t=this,{hasHoveredOverItemSinceOpened:s,opened:n}=t[We];if(n){const n=e.composedPath?e.composedPath()[0]:e.target,i=t.items;if(n&&n instanceof Node&&i){const e=P(i,n),r=i[e],o=r&&!r.disabled?e:-1;(s||o>=0)&&o!==t[We].currentIndex&&(t[Se]=!0,t[Fe]({currentIndex:o}),o>=0&&!s&&t[Fe]({hasHoveredOverItemSinceOpened:!0}),t[Se]=!1)}}}function Hn(e){e[We].opened&&e.isConnected?e[Rn]||(e[Rn]=Wn.bind(e),document.addEventListener("mousemove",e[Rn])):e[Rn]&&(document.removeEventListener("mousemove",e[Rn]),e[Rn]=null)}async function Nn(e){const t=e[Se],s=e[We].currentIndex>=0,n=e.items;if(n){const i=s?n[e[We].currentIndex]:void 0,r=e[We].popupList;s&&"flashCurrentItem"in r&&await r.flashCurrentItem();const o=e[Se];e[Se]=t,await e.close(i),e[Se]=o}}const zn=Bn(Mn);function Yn(e,t,s){if(!s||s.menuPartType){const{menuPartType:s}=t,n=e.getElementById("menu");n&&X(n,s)}}const Un=class extends zn{get[ee](){return Object.assign(super[ee],{menuPartType:en})}get items(){const e=this[we]&&this[we].menu;return e?e.items:null}get menuPartType(){return this[We].menuPartType}set menuPartType(e){this[Fe]({menuPartType:e})}[ke](e){if(super[ke](e),Yn(this[Me],this[We],e),e.menuPartType&&(this[we].menu.addEventListener("blur",(async e=>{const t=e.relatedTarget||document.activeElement;this.opened&&!v(this[we].menu,t)&&(this[Se]=!0,await this.close(),this[Se]=!1)})),this[we].menu.addEventListener("mousedown",(e=>{0===e.button&&this.opened&&(e.stopPropagation(),e.preventDefault())}))),e.opened){const{opened:e}=this[We];this[we].source.setAttribute("aria-expanded",e.toString())}}[Ae](e){super[Ae](e),e.menuPartType&&this[Fe]({popupList:this[we].menu})}[He](e,t){const s=super[He](e,t);return t.opened&&!e.opened&&Object.assign(s,{currentIndex:-1}),s}get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <div id="menu" part="menu">
          <slot></slot>
        </div>
      `),Yn(e.content,this[We]),e.content.append(A`
      <style>
        [part~="menu"] {
          max-height: 100%;
        }
      </style>
    `),e}},Vn=an($),$n=class extends Vn{get[ee](){return Object.assign(super[ee],{direction:"down"})}get direction(){return this[We].direction}set direction(e){this[Fe]({direction:e})}[ke](e){if(super[ke](e),e.direction){const{direction:e}=this[We];this[we].downIcon.style.display="down"===e?"block":"none",this[we].upIcon.style.display="up"===e?"block":"none"}}get[Je](){return D.html`
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
    `}};function qn(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{popupTogglePartType:$n})}get popupTogglePartType(){return this[We].popupTogglePartType}set popupTogglePartType(e){this[Fe]({popupTogglePartType:e})}[ke](e){if(super[ke](e),Gn(this[Me],this[We],e),e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[We],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.disabled){const{disabled:e}=this[We];this[we].popupToggle.disabled=e}}get[Je](){const e=super[Je],t=e.content.querySelector('[part~="source"]');return t&&t.append(A`
          <div
            id="popupToggle"
            part="popup-toggle"
            exportparts="toggle-icon, down-icon, up-icon"
            tabindex="-1"
          ></div>
      `),Gn(e.content,this[We]),e.content.append(A`
      <style>
        [part~="popup-toggle"] {
          outline: none;
        }

        [part~="source"] {
          align-items: center;
          display: flex;
        }
      </style>
    `),e}}}function Gn(e,t,s){if(!s||s.popupTogglePartType){const{popupTogglePartType:s}=t,n=e.getElementById("popupToggle");n&&X(n,s)}}const Kn=class extends fs{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          [part~="button"] {
            background: #eee;
            border: 1px solid #ccc;
            padding: 0.25em 0.5em;
          }
        </style>
      `),e}},Xn=class extends $n{get[Je](){const e=super[Je],t=e.content.getElementById("downIcon"),s=A`
      <svg
        id="downIcon"
        part="toggle-icon down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 0 l5 5 5 -5 z" />
      </svg>
    `.firstElementChild;t&&s&&K(t,s);const n=e.content.getElementById("upIcon"),i=A`
      <svg
        id="upIcon"
        part="toggle-icon up-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 5 l5 -5 5 5 z" />
      </svg>
    `.firstElementChild;return n&&i&&K(n,i),e.content.append(A`
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
      `),e}},Zn=class extends pn{},Jn=class extends gn{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 0px 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
          }
        </style>
      `),e}},Qn=class extends kn{get[ee](){return Object.assign(super[ee],{backdropPartType:Zn,framePartType:Jn})}};class _n extends(qn(Un)){get[ee](){return Object.assign(super[ee],{menuPartType:tn,popupPartType:Qn,popupTogglePartType:Xn,sourcePartType:Kn})}get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          [part~="menu"] {
            background: window;
            border: none;
            padding: 0.5em 0;
          }
        </style>
      `),e}}const ei=_n;function ti(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if("current"===e){const t=w(e,s);this.current!==t&&(this.current=t)}else super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{current:!1})}[ke](e){if(super[ke](e),e.current){const{current:e}=this[We];S(this,"current",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.current){const{current:e}=this[We],t=new CustomEvent("current-changed",{bubbles:!0,detail:{current:e}});this.dispatchEvent(t);const s=new CustomEvent("currentchange",{bubbles:!0,detail:{current:e}});this.dispatchEvent(s)}}get current(){return this[We].current}set current(e){this[Fe]({current:e})}}}customElements.define("elix-menu-button",class extends ei{});class si extends(ti(an(qt($)))){}const ni=si,ii=class extends ni{get[Je](){return D.html`
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
    `}};customElements.define("elix-menu-item",class extends ii{});const ri=class extends ${get disabled(){return!0}[ke](e){super[ke](e),this[ie]&&this.setAttribute("aria-hidden","true")}},oi=class extends ri{get[Je](){return D.html`
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
    `}};customElements.define("elix-menu-separator",class extends oi{});class ai extends(qn(Mn)){get[ee](){return Object.assign(super[ee],{popupPartType:Qn,sourcePartType:Kn})}}const li=ai;customElements.define("elix-popup-button",class extends li{}),customElements.define("elix-popup",class extends Qn{});const ci=Symbol("previousBodyStyleOverflow"),ui=Symbol("previousDocumentMarginRight");function di(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:"dialog"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super[Te]&&super[Te](e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.opened)if(this[We].opened&&document.documentElement){const e=document.documentElement.clientWidth,t=window.innerWidth-e;this[ci]=document.body.style.overflow,this[ui]=t>0?document.documentElement.style.marginRight:null,document.body.style.overflow="hidden",t>0&&(document.documentElement.style.marginRight=t+"px")}else null!=this[ci]&&(document.body.style.overflow=this[ci],this[ci]=null),null!=this[ui]&&(document.documentElement.style.marginRight=this[ui],this[ui]=null);if(e.role){const{role:e}=this[We];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}const hi=Symbol("wrap"),pi=Symbol("wrappingFocus");function gi(e){return class extends e{[Te](e){const t=T(this[Me]);if(t){const s=document.activeElement&&(document.activeElement===t||document.activeElement.contains(t)),n=this[Me].activeElement,i=n&&(n===t||v(n,t));(s||i)&&"Tab"===e.key&&e.shiftKey&&(this[pi]=!0,this[we].focusCatcher.focus(),this[pi]=!1)}return super[Te]&&super[Te](e)||!1}[ke](e){super[ke]&&super[ke](e),this[ie]&&this[we].focusCatcher.addEventListener("focus",(()=>{if(!this[pi]){const e=T(this[Me]);e&&e.focus()}}))}[hi](e){const t=A`
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
      `,s=t.getElementById("focusCaptureContainer");s&&(e.replaceWith(t),s.append(e))}}}gi.wrap=hi;const mi=gi,fi=class extends pn{constructor(){super(),"PointerEvent"in window||this.addEventListener("touchmove",(e=>{1===e.touches.length&&e.preventDefault()}))}},bi=di(mi(as(Tn))),yi=class extends bi{get[ee](){return Object.assign(super[ee],{backdropPartType:fi,tabIndex:-1})}get[Je](){const e=super[Je],t=e.content.querySelector("#frame");return this[mi.wrap](t),e.content.append(A`
        <style>
          :host {
            height: 100%;
            left: 0;
            pointer-events: initial;
            top: 0;
            width: 100%;
          }
        </style>
      `),e}},wi=class extends fi{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: rgba(0, 0, 0, 0.2);
          }
        </style>
      `),e}};function xi(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{backdropPartType:wi,framePartType:Jn})}}}class vi extends(xi(yi)){}const Ti=vi;function Ei(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectedText:""})}[oe](e){return super[oe]?super[oe](e):Hs(e)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?this[oe](i):"";Object.assign(s,{selectedText:r})}return s}get selectedText(){return this[We].selectedText}set selectedText(e){const{items:t}=this[We],s=t?function(e,t,s){return e.findIndex((e=>t(e)===s))}(t,this[oe],String(e)):-1;this[Fe]({selectedIndex:s})}}}function Pi(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{value:""})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?i.getAttribute("value"):"";Object.assign(s,{value:r})}return s}get value(){return this[We].value}set value(e){const{items:t}=this[We],s=t?function(e,t){return e.findIndex((e=>e.getAttribute("value")===t))}(t,String(e)):-1;this[Fe]({selectedIndex:s})}}}function Ii(e){return class extends e{attributeChangedCallback(e,t,s){"selected-index"===e?this.selectedIndex=Number(s):super.attributeChangedCallback(e,t,s)}[Ae](e){if(super[Ae]&&super[Ae](e),e.selectedIndex&&this[Se]){const e=this[We].selectedIndex,t=new CustomEvent("selected-index-changed",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedindexchange",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(s)}}get selectedIndex(){const{items:e,selectedIndex:t}=this[We];return e&&e.length>0?t:-1}set selectedIndex(e){isNaN(e)||this[Fe]({selectedIndex:e})}get selectedItem(){const{items:e,selectedIndex:t}=this[We];return e&&e[t]}set selectedItem(e){const{items:t}=this[We];if(!t)return;const s=t.indexOf(e);s>=0&&this[Fe]({selectedIndex:s})}}}customElements.define("elix-dialog",class extends Ti{});const Ci=Ls(st(rs(Ms(Rs(Bn(Ei(Pi(Ii(Js(Mn))))))))));function Si(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&X(n,s)}if(!s||s.valuePartType){const{valuePartType:s}=t,n=e.getElementById("value");n&&X(n,s)}}const ki=class extends Ci{[Z](e,t){L(t,(e?[...e.childNodes]:[]).map((e=>e.cloneNode(!0))))}get[ee](){return Object.assign(super[ee],{ariaHasPopup:"listbox",listPartType:en,selectedIndex:-1,selectedItem:null,valuePartType:"div"})}get items(){const e=this[we]&&this[we].list;return e?e.items:null}get listPartType(){return this[We].listPartType}set listPartType(e){this[Fe]({listPartType:e})}[ke](e){if(super[ke](e),Si(this[Me],this[We],e),e.items||e.selectedIndex){const{items:e,selectedIndex:t}=this[We],s=e?e[t]:null;this[Z](s,this[we].value),e&&e.forEach((e=>{"selected"in e&&(e.selected=e===s)}))}if(e.opened){const{opened:e}=this[We];this[we].source.setAttribute("aria-expanded",e.toString())}if(e.sourcePartType){const e=this[we].source;e.inner&&e.inner.setAttribute("role","none")}}[Ae](e){super[Ae](e),e.listPartType&&this[Fe]({popupList:this[we].list})}[He](e,t){const s=super[He](e,t);if(t.opened&&e.opened&&Object.assign(s,{currentIndex:e.selectedIndex}),t.opened){const{closeResult:n,currentIndex:i,opened:r}=e,o=t.opened&&!r,a=n&&n.canceled;o&&!a&&i>=0&&Object.assign(s,{selectedIndex:i})}if(t.items||t.selectedIndex){const{items:t,opened:n,selectedIndex:i}=e;!n&&i<0&&t&&t.length>0&&Object.assign(s,{selectedIndex:0})}return s}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="source"]');t&&K(t,A` <div id="value" part="value"></div> `);const s=e.content.querySelector("slot:not([name])");s&&s.replaceWith(A`
        <div id="list" part="list">
          <slot></slot>
        </div>
      `);const n=e.content.querySelector('[part~="source"]');return n&&(n.setAttribute("aria-activedescendant","value"),n.setAttribute("aria-autocomplete","none"),n.setAttribute("aria-controls","list"),n.setAttribute("role","combobox")),Si(e.content,this[We]),e.content.append(A`
      <style>
        [part~="list"] {
          max-height: 100%;
        }
      </style>
    `),e}get valuePartType(){return this[We].valuePartType}set valuePartType(e){this[Fe]({valuePartType:e})}},Li=dn(tt(Ls(js(Fs(at(rs(Ms(Rs(Ns(os(as(zs(qs(ls(Ii(Ei(Pi(Js(Qs($)))))))))))))))))))),Oi=class extends Li{get[ee](){return Object.assign(super[ee],{highlightCurrentItem:!0,orientation:"vertical",role:"listbox"})}async flashCurrentItem(){const e=this[We].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Fe]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Fe]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[ke](e){if(super[ke](e),e.items||e.currentIndex||e.highlightCurrentItem){const{currentIndex:e,items:t,highlightCurrentItem:s}=this[We];t&&t.forEach(((t,n)=>{const i=n===e;t.toggleAttribute("current",s&&i),t.setAttribute("aria-selected",String(i))}))}}get[je](){return this[we].container}get[Je](){const e=super[Je];return e.content.append(A`
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
    `),e}},Ai=class extends Oi{get[Je](){const e=super[Je];return e.content.append(A`
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
    `),e}};class Di extends(qn(ki)){get[ee](){return Object.assign(super[ee],{listPartType:Ai,popupPartType:Qn,sourcePartType:Kn,popupTogglePartType:Xn})}}const ji=Di;customElements.define("elix-dropdown-list",class extends ji{});class Fi extends(dn(ti(an(qt($))))){get[ee](){return Object.assign(super[ee],{role:"option"})}get[Je](){return D.html`
      <style>
        :host {
          display: block;
        }
      </style>
      <slot></slot>
    `}}const Mi=Fi,Ri=class extends Mi{get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
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

    `),e}};customElements.define("elix-option",class extends Ri{});const Bi=Symbol("deferToScrolling"),Wi=Symbol("multiTouch"),Hi=Symbol("previousTime"),Ni=Symbol("previousVelocity"),zi=Symbol("previousX"),Yi=Symbol("previousY"),Ui=Symbol("startX"),Vi=Symbol("startY"),$i=Symbol("touchSequenceAxis");function qi(e){return"pen"===e.pointerType||"touch"===e.pointerType&&e.isPrimary}function Gi(e,t,s,n){const i=e,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[We],l=t-i[zi],c=s-i[Yi],u=Date.now(),d="vertical"===r?c:l,h=d/(u-i[Hi])*1e3;i[zi]=t,i[Yi]=s,i[Hi]=u,i[Ni]=h;const p=Math.abs(c)>Math.abs(l)?"vertical":"horizontal";if(null===i[$i])i[$i]=p;else if(p!==i[$i])return!0;if(p!==r)return!1;if(i[Bi]&&Os(n,r,d<0))return!1;i[Ui]||(i[Ui]=t),i[Vi]||(i[Vi]=s);const g=function(e,t,s){const{swipeAxis:n}=e[We],i=e,r="vertical"===n,o=r?s-i[Vi]:t-i[Ui],a=r?e[Xe].offsetHeight:e[Xe].offsetWidth;return a>0?o/a:0}(e,t,s),m=Math.max(Math.min(g,o),a);return e[We].swipeFraction!==m&&(i[Bi]=!1,e[Fe]({swipeFraction:m}),!0)}function Ki(e,t,s,n){const i=e[Ni],{swipeAxis:r,swipeFraction:o}=e[We],a="vertical"===r;let l=!1;if(e[Bi]&&(l=Os(n,r,i<0)),!l){let t;if(i>=800&&o>=0?(t=!0,a?e[Fe]({swipeDownWillCommit:!0}):e[Fe]({swipeRightWillCommit:!0})):i<=-800&&o<=0?(t=!1,a?e[Fe]({swipeUpWillCommit:!0}):e[Fe]({swipeLeftWillCommit:!0})):e[We].swipeLeftWillCommit||e[We].swipeUpWillCommit?t=!1:(e[We].swipeRightWillCommit||e[We].swipeDownWillCommit)&&(t=!0),void 0!==t){const s=a?t?Ne:qe:t?Ve:Ye;s&&e[s]&&e[s]()}}e[$i]=null,e[Fe]({swipeFraction:null})}function Xi(e,t,s){const n=e;n[Bi]=!0,n[Hi]=Date.now(),n[Ni]=0,n[zi]=t,n[Yi]=s,n[Ui]=null,n[Vi]=null,n[$i]=null,e[Fe]({swipeFraction:0}),e[Ke]&&e[Ke](t,s)}const Zi=Symbol("absorbDeceleration"),Ji=Symbol("deferToScrolling"),Qi=Symbol("lastDeltaX"),_i=Symbol("lastDeltaY"),er=Symbol("lastWheelTimeout"),tr=Symbol("postGestureDelayComplete"),sr=Symbol("wheelDistance"),nr=Symbol("wheelSequenceAxis");function ir(e){const t=e;t[Zi]=!1,t[Ji]=!0,t[Qi]=0,t[_i]=0,t[tr]=!0,t[sr]=0,t[nr]=null,t[er]&&(clearTimeout(t[er]),t[er]=null)}const rr=di(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{enableEffects:!1})}[Ae](e){super[Ae]&&super[Ae](e),this[ie]&&setTimeout((()=>{this[Fe]({enableEffects:!0})}))}}}(mi(as(ls(function(e){return class extends e{[ke](e){super[ke]&&super[ke](e),this[ie]&&("TouchEvent"in window?(this.addEventListener("touchstart",(async e=>{if(this[Se]=!0,!this[Wi]){if(1===e.touches.length){const{clientX:t,clientY:s}=e.changedTouches[0];Xi(this,t,s)}else this[Wi]=!0;await Promise.resolve(),this[Se]=!1}})),this.addEventListener("touchmove",(async e=>{if(this[Se]=!0,!this[Wi]&&1===e.touches.length&&e.target){const{clientX:t,clientY:s}=e.changedTouches[0];Gi(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Se]=!1})),this.addEventListener("touchend",(async e=>{if(this[Se]=!0,0===e.touches.length&&e.target){if(!this[Wi]){const{clientX:t,clientY:s}=e.changedTouches[0];Ki(this,0,0,e.target)}this[Wi]=!1}await Promise.resolve(),this[Se]=!1}))):"PointerEvent"in window&&(this.addEventListener("pointerdown",(async e=>{if(this[Se]=!0,qi(e)){const{clientX:t,clientY:s}=e;Xi(this,t,s)}await Promise.resolve(),this[Se]=!1})),this.addEventListener("pointermove",(async e=>{if(this[Se]=!0,qi(e)&&e.target){const{clientX:t,clientY:s}=e;Gi(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Se]=!1})),this.addEventListener("pointerup",(async e=>{if(this[Se]=!0,qi(e)&&e.target){const{clientX:t,clientY:s}=e;Ki(this,0,0,e.target)}await Promise.resolve(),this[Se]=!1}))),this.style.touchAction="TouchEvent"in window?"manipulation":"none")}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeStartX:null,swipeStartY:null,swipeUpWillCommit:!1})}get[Xe](){return super[Xe]||this}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{constructor(){super(),this.addEventListener("wheel",(async e=>{this[Se]=!0,function(e,t){const s=e;s[er]&&clearTimeout(s[er]),s[er]=setTimeout((async()=>{e[Se]=!0,async function(e){let t;e[We].swipeDownWillCommit?t=Ne:e[We].swipeLeftWillCommit?t=Ye:e[We].swipeRightWillCommit?t=Ve:e[We].swipeUpWillCommit&&(t=qe),ir(e),e[Fe]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1}),t&&e[t]&&await e[t]()}(e),await Promise.resolve(),s[Se]=!1}),100);const n=t.deltaX,i=t.deltaY,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[We],l="vertical"===r,c=l?Math.sign(i)*(i-s[_i]):Math.sign(n)*(n-s[Qi]);s[Qi]=n,s[_i]=i;const u=null===s[nr],d=Math.abs(i)>Math.abs(n)?"vertical":"horizontal";if(!u&&d!==s[nr])return!0;if(d!==r)return!1;if(!s[tr])return!0;if(c>0)s[Zi]=!1;else if(s[Zi])return!0;if(s[Ji]&&Os(e[je]||e,r,(l?i:n)>0))return!1;s[Ji]=!1,u&&(s[nr]=d,e[Ke]&&e[Ke](t.clientX,t.clientY)),s[sr]-=l?i:n;const h=l?s[Xe].offsetHeight:s[Xe].offsetWidth;let p=h>0?s[sr]/h:0;p=Math.sign(p)*Math.min(Math.abs(p),1);const g=Math.max(Math.min(p,o),a);let m;return-1===g?m=l?qe:Ye:1===g&&(m=l?Ne:Ve),m?function(e,t){e[t]&&e[t]();const s=e;s[Zi]=!0,s[Ji]=!0,s[tr]=!1,s[sr]=0,s[nr]=null,setTimeout((()=>{s[tr]=!0}),250),e[Fe]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}(e,m):e[Fe]({swipeFraction:g}),!0}(this,e)&&(e.preventDefault(),e.stopPropagation()),await Promise.resolve(),this[Se]=!1})),ir(this)}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}get[Xe](){return super[Xe]||this}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{get[ne](){return super[ne]||this}[ke](e){super[ke]&&super[ke](e),this[ie]&&(this[ne]===this?this:this[Me]).addEventListener("transitionend",(e=>{const t=this[We].effectEndTarget||this[ne];e.target===t&&this[Fe]({effectPhase:"after"})}))}[Ae](e){if(super[Ae]&&super[Ae](e),e.effect||e.effectPhase){const{effect:e,effectPhase:t}=this[We],s=new CustomEvent("effect-phase-changed",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(s);const n=new CustomEvent("effectphasechange",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(n),e&&("after"!==t&&this.offsetHeight,"before"===t&&this[Fe]({effectPhase:"during"}))}}async[Be](e){await this[Fe]({effect:e,effectPhase:"before"})}}}(Tn))))))));async function or(e){e[Fe]({effect:"close",effectPhase:"during"}),await e.close()}async function ar(e){e[Fe]({effect:"open",effectPhase:"during"}),await e.open()}const lr=class extends rr{get[ee](){return Object.assign(super[ee],{backdropPartType:fi,drawerTransitionDuration:250,fromEdge:"start",gripSize:null,openedFraction:0,openedRenderedFraction:0,persistent:!0,role:"landmark",showTransition:!1,tabIndex:-1})}get[ne](){return this[we].frame}get fromEdge(){return this[We].fromEdge}set fromEdge(e){this[Fe]({fromEdge:e})}get gripSize(){return this[We].gripSize}set gripSize(e){this[Fe]({gripSize:e})}[ke](e){if(super[ke](e),e.backdropPartType&&this[we].backdrop.addEventListener("click",(async()=>{this[Se]=!0,await this.close(),this[Se]=!1})),e.gripSize||e.opened||e.swipeFraction){const{gripSize:e,opened:t,swipeFraction:s}=this[We],n=null!==s,i=t||n;this.style.pointerEvents=i?"initial":"none";const r=!(null!==e||i);this[we].frame.style.clipPath=r?"inset(0px)":""}if(e.effect||e.effectPhase||e.fromEdge||e.gripSize||e.openedFraction||e.rightToLeft||e.swipeFraction){const{drawerTransitionDuration:e,effect:t,effectPhase:s,fromEdge:n,gripSize:i,openedFraction:r,openedRenderedFraction:o,rightToLeft:a,showTransition:l,swipeFraction:c}=this[We],u="left"===n||"top"===n||"start"===n&&!a||"end"===n&&a?-1:1,d=u*(1-r);null!==c||"open"===t&&"before"===s?this[we].backdrop.style.visibility="visible":"close"===t&&"after"===s&&(this[we].backdrop.style.visibility="hidden");const h=Math.abs(r-o),p=l?h*(e/1e3):0,g=100*d+"%",m=i?i*-u*(1-r):0,f=`translate${"top"===n||"bottom"===n?"Y":"X"}(${0===m?g:`calc(${g} + ${m}px)`})`;Object.assign(this[we].frame.style,{transform:f,transition:l?`transform ${p}s`:""})}if(e.fromEdge||e.rightToLeft){const{fromEdge:e,rightToLeft:t}=this[We],s={bottom:0,left:0,right:0,top:0},n={bottom:"top",left:"right",right:"left",top:"bottom"};n.start=n[t?"right":"left"],n.end=n[t?"left":"right"],Object.assign(this.style,s,{[n[e]]:null});const i={bottom:"flex-end",end:"flex-end",left:t?"flex-end":"flex-start",right:t?"flex-start":"flex-end",start:"flex-start",top:"flex-start"};this.style.flexDirection="top"===e||"bottom"===e?"column":"row",this.style.justifyContent=i[e]}e.opened&&this.setAttribute("aria-expanded",this[We].opened.toString())}[Ae](e){super[Ae](e),e.opened&&S(this,"opened",this[We].opened),e.openedFraction&&this[Fe]({openedRenderedFraction:this[We].openedFraction})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.fromEdge){const{fromEdge:t}=e,n="top"===t||"bottom"===t?"vertical":"horizontal";Object.assign(s,{swipeAxis:n})}if(t.effect||t.effectPhase||t.fromEdge||t.rightToLeft||t.swipeFraction){const{effect:t,effectPhase:n,fromEdge:i,rightToLeft:r,swipeFraction:o}=e,a="open"===t&&"before"!==n||"close"===t&&"before"===n,l="left"===i||"top"===i||"start"===i&&!r||"end"===i&&r,c=.999,u=l&&!a||!l&&a,d=u?0:-c,h=u?c:0,p=l?-1:1;let g=a?1:0;null!==o&&(g-=p*Math.max(Math.min(o,h),d)),Object.assign(s,{openedFraction:g})}if(t.enableEffects||t.effect||t.effectPhase||t.swipeFraction){const{enableEffects:t,effect:n,effectPhase:i,swipeFraction:r}=e,o=null!==r,a=t&&!o&&n&&("during"===i||"after"===i);Object.assign(s,{showTransition:a})}return s}async[Ne](){const{fromEdge:e}=this[We];"top"===e?ar(this):"bottom"===e&&or(this)}async[Ye](){const{fromEdge:e,rightToLeft:t}=this[We],s="left"===e||"start"===e&&!t||"end"===e&&t;"right"===e||"start"===e&&t||"end"===e&&!t?ar(this):s&&or(this)}async[Ve](){const{fromEdge:e,rightToLeft:t}=this[We],s="right"===e||"start"===e&&t||"end"===e&&!t;"left"===e||"start"===e&&!t||"end"===e&&t?ar(this):s&&or(this)}async[qe](){const{fromEdge:e}=this[We];"bottom"===e?ar(this):"top"===e&&or(this)}get[je](){return this[we].frame}get[Xe](){return this[we].frame}get[Je](){const e=super[Je],t=e.content.querySelector("#frameContent");return this[mi.wrap](t),e.content.append(A`
        <style>
          :host {
            align-items: stretch;
            -webkit-overflow-scrolling: touch; /* for momentum scrolling */
          }

          [part~="backdrop"] {
            will-change: opacity;
          }

          [part~="frame"] {
            overflow: auto;
            will-change: transform;
          }

          :host([opened="false"]) [part~="frame"] {
            overflow: hidden;
          }
        </style>
      `),e}};class cr extends(function(e){return class extends e{[ke](e){if(super[ke]&&super[ke](e),e.openedFraction){const{drawerTransitionDuration:e,openedFraction:t,openedRenderedFraction:s,showTransition:n}=this[We],i=Math.abs(t-s),r=n?i*(e/1e3):0;Object.assign(this[we].backdrop.style,{opacity:t,transition:n?`opacity ${r}s linear`:""})}}}}(xi(lr))){}const ur=cr;function dr(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectionEnd:null,selectionStart:null})}[ke](e){if(super[ke]&&super[ke](e),this[ie]){const e=(()=>{setTimeout((()=>{this[Se]=!0,function(e){const t=e.inner,{selectionEnd:s,selectionStart:n}=t;e[Fe]({selectionEnd:s,selectionStart:n})}(this),this[Se]=!1}),10)}).bind(this);this.addEventListener("keydown",e),this.addEventListener("mousedown",e)}}[Ae](e){super[Ae]&&super[Ae](e);const{selectionEnd:t,selectionStart:s}=this[We];null===t&&this[Fe]({selectionEnd:this.inner.selectionEnd}),null===s&&this[Fe]({selectionStart:this.inner.selectionStart})}[He](e,t){const s=super[He]?super[He](e,t):{};return t.value&&Object.assign(s,{selectionStart:null,selectionEnd:null}),s}}}customElements.define("elix-drawer",class extends ur{});const hr=st(at(rs(dr(vt.wrap("input"))))),pr=class extends hr{get[ee](){return Object.assign(super[ee],{valueCopy:""})}get[xe](){return this.inner}[ke](e){super[ke](e),this[ie]&&this[we].inner.addEventListener("input",(()=>{this[Se]=!0,this.value=this.inner.value,this[Se]=!1}))}get[Je](){const e=super[Je];return e.content.append(A`
      <style>
        [part~="input"] {
          font: inherit;
          outline: none;
          text-align: inherit;
        }
      </style>
    `),e}get value(){return super.value}set value(e){const t=String(e);super.value=t,this[Fe]({valueCopy:t})}},gr=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"option",role:e.role||"listbox"})}get itemRole(){return this[We].itemRole}set itemRole(e){this[Fe]({itemRole:e})}[ke](e){super[ke]&&super[ke](e);const{itemRole:t}=this[We],s=this[We].items;if(e.items&&s&&s.forEach((e=>{e.id||(e.id=ks(e))})),(e.items||e.itemRole)&&s&&s.forEach((e=>{t===Ss[e.localName]?e.removeAttribute("role"):e.setAttribute("role",t)})),e.items||e.selectedIndex||e.selectedItemFlags){const{selectedItemFlags:e,selectedIndex:t}=this[We];s&&s.forEach(((s,n)=>{const i=e?e[n]:n===t;s.setAttribute("aria-selected",i.toString())}))}if(e.items||e.selectedIndex){const{selectedIndex:e}=this[We],t=e>=0&&s?s[e]:null;t?(t.id||(t.id=ks(t)),this.setAttribute("aria-activedescendant",t.id)):this.removeAttribute("aria-activedescendant")}if(e.selectedItemFlags&&(this[We].selectedItemFlags?this.setAttribute("aria-multiselectable","true"):this.removeAttribute("aria-multiselectable")),e.orientation){const{orientation:e}=this[We];this.setAttribute("aria-orientation",e)}if(e.role){const{role:e}=this[We];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(tt(Ls(js(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectedIndex:-1,selectedItem:null})}[He](e,t){const s=super[He]?super[He](e,t):{};return t.currentIndex?Object.assign(s,{selectedIndex:e.currentIndex}):t.selectedIndex&&Object.assign(s,{currentIndex:e.selectedIndex}),t.currentItem?Object.assign(s,{selectedItem:e.currentItem}):t.selectedItem&&Object.assign(s,{currentItem:e.selectedItem}),s}}}(Fs(at(rs(Ms(Rs(Ns(os(as(zs(qs(ls(Ii(Ei(Pi(Js(Qs($))))))))))))))))))))),mr=class extends gr{get[ee](){return Object.assign(super[ee],{orientation:"vertical"})}get orientation(){return this[We].orientation}set orientation(e){this[Fe]({orientation:e})}[ke](e){if(super[ke](e),e.items||e.currentIndex){const{currentIndex:e,items:t}=this[We];t&&t.forEach(((t,s)=>{t.toggleAttribute("selected",s===e)}))}if(e.orientation){const e="vertical"===this[We].orientation?{display:"block",flexDirection:"",overflowX:"hidden",overflowY:"auto"}:{display:"flex",flexDirection:"row",overflowX:"auto",overflowY:"hidden"};Object.assign(this[we].container.style,e)}}get[je](){return this[we].container}get[Je](){const e=super[Je];return e.content.append(A`
      <style>
        :host {
          box-sizing: border-box;
          cursor: default;
          display: flex;
          overflow: hidden; /* Container element is responsible for scrolling */
          -webkit-tap-highlight-color: transparent;
        }

        #container {
          display: flex;
          flex: 1;
          -webkit-overflow-scrolling: touch; /* for momentum scrolling */
        }
      </style>
      <div id="container" role="none">
        <slot id="slot"></slot>
      </div>
    `),e}},fr=class extends pr{get[ee](){return Object.assign(super[ee],{autoCompleteSelect:!1,opened:!1,originalText:"",textIndex:-1,texts:[]})}[Ee](e,t){if(0===t.length||!e)return null;const s=t.toLowerCase();return e.find((e=>e.toLowerCase().startsWith(s)))||null}get opened(){return this[We].opened}set opened(e){this[Fe]({opened:e})}[ke](e){if(super[ke](e),this[ie]&&(this[we].inner.addEventListener("input",(()=>{setTimeout((()=>{this[Se]=!0;const e=this.inner,t=this.value.toLowerCase(),s=e.selectionStart===t.length&&e.selectionEnd===t.length,n=this[We].originalText,i=t.startsWith(n)&&t.length===n.length+1;s&&i&&function(e){const t=e[Ee](e.texts,e.value);t&&e[Fe]({autoCompleteSelect:!0,value:t})}(this),this[Fe]({originalText:t}),this[Se]=!1}))})),X(this[we].accessibleList,mr)),e.opened){const{opened:e}=this[We];this[we].inner.setAttribute("aria-expanded",e.toString())}if(e.texts){const{texts:e}=this[We],t=null===e?[]:e.map((e=>{const t=document.createElement("div");return t.textContent=e,t}));L(this[we].accessibleList,t)}if(e.textIndex){const{textIndex:e}=this[We],t=this[we].accessibleList;"currentIndex"in t&&(t.currentIndex=e);const s=t.currentItem,n=s?s.id:null;n?this[xe].setAttribute("aria-activedescendant",n):this[xe].removeAttribute("aria-activedescendant")}}[Ae](e){super[Ae](e);const{autoCompleteSelect:t,originalText:s}=this[We];if(e.originalText&&t){this[Fe]({autoCompleteSelect:!1,selectionEnd:this[We].value.length,selectionStart:s.length});const e=new(window.InputEvent||Event)("input",{detail:{originalText:s}});this.dispatchEvent(e)}}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.valueCopy){const{texts:t,valueCopy:n}=e,i=t.indexOf(n);Object.assign(s,{textIndex:i})}return s}get[Je](){const e=super[Je],t=e.content.querySelector('[part~="input"]');return t&&(t.setAttribute("aria-autocomplete","both"),t.setAttribute("aria-controls","accessibleList"),t.setAttribute("role","combobox")),e.content.append(A`
      <style>
        #accessibleList {
          height: 0;
          position: absolute;
          width: 0;
        }
      </style>
      <div id="accessibleList" tabindex="-1"></div>
    `),e}get texts(){return this[We].texts}set texts(e){this[Fe]({texts:e})}get value(){return super.value}set value(e){super.value=e,this[Me]&&!this.inner.matches(":focus")&&this[Fe]({originalText:e})}},br=class extends ${[ke](e){super[ke](e),this[ie]&&this.setAttribute("hidden","")}},yr=ct(st(function(e){return class extends e{select(){this[xe].select()}get selectionEnd(){return this[xe].selectionEnd}set selectionEnd(e){this[xe].selectionEnd=e}get selectionStart(){return this[xe].selectionStart}set selectionStart(e){this[xe].selectionStart=e}setRangeText(...e){this[xe].setRangeText(...e)}setSelectionRange(...e){this[xe].setSelectionRange(...e),document.createElement("input").select}}}(at(rs(as(nn(qn(jn))))))));function wr(e,t,s){if(!s||s.inputPartType){const{inputPartType:s}=t,n=e.getElementById("input");n&&X(n,s)}}function xr(e,t){const s=e[ve];if(!s[t])return!1;const n=s[t]();if(n){const t=s.currentIndex;e[Fe]({currentIndex:t})}return n}const vr=Symbol("itemsChangedListener"),Tr=Symbol("previousItemsDelegate"),Er=Symbol("currentIndexChangedListener"),Pr=Ls(function(e){return class extends e{[ce](){return xr(this,ce)}[ue](){return xr(this,ue)}[he](){return xr(this,he)}[pe](){return xr(this,pe)}}}(function(e){return class extends e{constructor(){super(),this[vr]=e=>{const t=e.target.items;this[We].items!==t&&this[Fe]({items:t})},this[Er]=e=>{const t=e.detail.currentIndex;this[We].currentIndex!==t&&this[Fe]({currentIndex:t})}}get[ee](){return Object.assign(super[ee]||{},{items:null})}get items(){return this[We]?this[We].items:null}[ke](e){if(super[ke]&&super[ke](e),e.currentIndex){if(void 0===this[ve])throw`To use DelegateItemsMixin, ${this.constructor.name} must define a getter for [itemsDelegate].`;"currentIndex"in this[ve]&&(this[ve].currentIndex=this[We].currentIndex)}}[Ae](e){super[Ae]&&super[Ae](e);const t=this[Tr];this[ve]!==t&&(t&&(t.removeEventListener(this[vr]),t.removeEventListener(this[Er])),this[ve].addEventListener("itemschange",this[vr]),this[ve].addEventListener("currentindexchange",this[Er]))}}}(Bn(Ii(class extends yr{get[ee](){return Object.assign(super[ee],{ariaHasPopup:null,confirmedValue:"",focused:!1,inputPartType:"input",orientation:"vertical",placeholder:"",selectText:!1,value:""})}get[xe](){return this[we].input}get input(){return this[Me]?this[we].input:null}get inputPartType(){return this[We].inputPartType}set inputPartType(e){this[Fe]({inputPartType:e})}[Te](e){let t;switch(e.key){case"ArrowDown":case"ArrowUp":case"PageDown":case"PageUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0);break;case"Escape":this.close({canceled:"Escape"}),t=!0;break;case"F4":this.opened?this.close({canceled:"F4"}):this.open(),t=!0}return t||super[Te]&&super[Te](e)}get placeholder(){return this[We].placeholder}set placeholder(e){this[Fe]({placeholder:String(e)})}[ke](e){if(super[ke](e),wr(this[Me],this[We],e),e.inputPartType&&(this[we].input.addEventListener("blur",(()=>{this[Fe]({focused:!1}),this.opened&&(this[Se]=!0,this.close(),this[Se]=!1)})),this[we].input.addEventListener("focus",(()=>{this[Se]=!0,this[Fe]({focused:!0}),this[Se]=!1})),this[we].input.addEventListener("input",(()=>{this[Se]=!0;const e=this[we].input.value,t={value:e,selectText:!1};this.closed&&e>""&&(t.opened=!0),this[Fe](t),this[Se]=!1})),this[we].input.addEventListener("keydown",(()=>{this[Se]=!0,this[Fe]({selectText:!1}),this[Se]=!1})),this[we].input.addEventListener("mousedown",(e=>{0===e.button&&(this[Se]=!0,this[Fe]({selectText:!1}),this.closed&&!this.disabled&&this.open(),this[Se]=!1)}))),e.opened||e.inputPartType){const e=this[we].input;if("opened"in e){const{opened:t}=this[We];e.opened=t}}if(e.popupTogglePartType){const e=this[we].popupToggle,t=this[we].input;e.addEventListener("mousedown",(e=>{0===e.button&&(this[We].disabled?e.preventDefault():(this[Se]=!0,this.toggle(),this[Se]=!1))})),e instanceof HTMLElement&&t instanceof HTMLElement&&E(e,t)}if(e.popupPartType){const e=this[we].popup,t=e;e.removeAttribute("tabindex"),"backdropPartType"in e&&(t.backdropPartType=br),"autoFocus"in e&&(t.autoFocus=!1);const s=t.frame;s&&Object.assign(s.style,{display:"flex",flexDirection:"column"}),"closeOnWindowResize"in e&&(t.closeOnWindowResize=!1)}if(e.disabled){const{disabled:e}=this[We];this[we].input.disabled=e,this[we].popupToggle.disabled=e}if(e.placeholder){const{placeholder:e}=this[We];this[we].input.placeholder=e}if(e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[We],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.value){const{value:e}=this[We];this[we].input.value=e}}[Ae](e){super[Ae](e),this[We].selectText&&setTimeout((()=>{if(this[We].selectText){const e=this[we].input;e.value>""&&(e.selectionStart=0,e.selectionEnd=e.value.length)}}))}[He](e,t){const s=super[He](e,t);if(t.opened||t.value){const{closeResult:t,opened:n}=e;n||(t&&t.canceled?Object.assign(s,{value:e.confirmedValue}):Object.assign(s,{confirmedValue:e.value}))}if(t.opened&&!e.opened){const e=!matchMedia("(pointer: coarse)").matches;Object.assign(s,{selectText:e})}return s}get[Je](){const e=super[Je],t=e.content.querySelector('slot[name="source"]');return t&&t.replaceWith(A`
        <input id="input" part="input"></input>
      `),wr(e.content,this[We]),e.content.append(A`
        <style>
          [part~="source"] {
            background-color: inherit;
            display: inline-grid;
            grid-template-columns: 1fr auto;
            position: relative;
          }

          [part~="input"] {
            outline: none;
          }

          [part~="popup"] {
            flex-direction: column;
            max-height: 100vh;
            max-width: 100vh;
          }
        </style>
      `),e}get value(){return this[We].value}set value(e){this[Fe]({value:e})}})))));function Ir(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&X(n,s)}}const Cr=Ns(class extends Pr{get[ee](){return Object.assign(super[ee],{currentIndex:-1,horizontalAlign:"stretch",listPartType:mr,selectedIndex:-1,selectedItem:null})}[oe](e){return Hs(e)}[Te](e){let t;const s=this[we].list;switch(e.key){case"ArrowDown":this.opened&&(t=e.altKey?this[ue]():this[he]());break;case"ArrowUp":this.opened&&(t=e.altKey?this[ce]():this[pe]());break;case"PageDown":this.opened&&(t=s.pageDown&&s.pageDown());break;case"PageUp":this.opened&&(t=s.pageUp&&s.pageUp())}if(t){const{selectedIndex:e}=this[We];e!==s.currentIndex&&this[Fe]({selectedIndex:s.currentIndex})}return t||super[Te]&&super[Te](e)}get listPartType(){return this[We].listPartType}set listPartType(e){this[Fe]({listPartType:e})}get[ve](){return this[we].list}[ke](e){if(e.listPartType&&this[we].list&&E(this[we].list,null),super[ke](e),Ir(this[Me],this[We],e),e.listPartType){const e=this[we].list;e instanceof HTMLElement&&E(e,this)}}[Ae](e){super[Ae](e),e.listPartType&&this[Fe]({popupList:this[we].list})}get selectedItemValue(){const{items:e,selectedIndex:t}=this[We],s=e?e[t]:null;return s?s.getAttribute("value"):""}set selectedItemValue(e){const{items:t}=this[We],s=String(e),n=t.findIndex((e=>e.getAttribute("value")===s));this[Fe]({selectedIndex:n})}[He](e,t){const s=super[He](e,t);if(t.selectedIndex&&Object.assign(s,{currentIndex:e.selectedIndex}),t.selectedItem&&Object.assign(s,{currentItem:e.selectedItem}),t.items||t.value){const{value:t}=e,n=e.items;if(n&&null!=t){const e=t.toLowerCase(),i=n.findIndex((t=>this[oe](t).toLowerCase()===e));Object.assign(s,{currentIndex:i})}}if(t.selectedIndex){const{items:t,selectedIndex:n,value:i}=e,r=t?t[n]:null,o=r?this[oe](r):"",a=!matchMedia("(pointer: coarse)").matches;i!==o&&Object.assign(s,{selectText:a,value:o})}if(t.opened){const{closeResult:n,currentIndex:i,opened:r}=e,o=t.opened&&!r,a=n&&n.canceled;o&&!a&&i>=0&&Object.assign(s,{selectedIndex:i})}return t.items&&Object.assign(s,{popupMeasured:!1}),s}get[Je](){const e=super[Je],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <div id="list" part="list" tabindex="-1">
          <slot></slot>
        </div>
      `),e.content.append(A`
      <style>
        [part~="list"] {
          border: none;
          flex: 1;
          height: 100%;
          max-height: 100%;
          overscroll-behavior: contain;
          width: 100%;
        }
      </style>
    `),Ir(e.content,this[We]),e}});function Sr(e){return function(e){return e.normalize("NFD").replace(/[\u0300-\u036f]/g,"")}(e).toLowerCase()}const kr=class extends mr{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,filter:null})}get filter(){return this[We].filter}set filter(e){const t=this[Se];this[Se]=!0,this[Fe]({filter:String(e)}),this[Se]=t}highlightTextInItem(e,t){const s=t.textContent||"",n=e?Sr(s).indexOf(Sr(e)):-1;if(n>=0){const t=n+e.length,i=s.substr(0,n),r=s.substring(n,t),o=s.substr(t),a=document.createDocumentFragment(),l=document.createElement("strong");return l.textContent=r,a.append(new Text(i),l,new Text(o)),a.childNodes}return[new Text(s)]}itemMatchesFilter(e,t){const s=this[oe](e);if(t){if(s){const e=Sr(s),n=Sr(t);return e.includes(n)}return!1}return!0}[ke](e){if(super[ke](e),e.filter||e.items){const{filter:e,availableItemFlags:t,items:s}=this[We];s&&s.forEach(((s,n)=>{const i=t[n];s.style.display=i?"":"none",i&&L(s,this.highlightTextInItem(e,s))}))}}[Ae](e){super[Ae](e),e.filter&&this.scrollCurrentItemIntoView()}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.filter||t.items){const{filter:t,items:n}=e,i=null===n?null:n.map((e=>this.itemMatchesFilter(e,t)));Object.assign(s,{availableItemFlags:i})}return s}},Lr=Zs(class extends Cr{get[ee](){return Object.assign(super[ee],{inputPartType:fr})}[ke](e){super[ke](e),e.texts&&"texts"in this[we].input&&(this[we].input.texts=this[We].texts)}}),Or=class extends Lr{get[ee](){return Object.assign(super[ee],{filter:"",listPartType:kr})}[ke](e){if(super[ke](e),e.inputPartType&&this[we].input.addEventListener("input",(e=>{this[Se]=!0;const t=e,s=t.detail?t.detail.originalText:this[We].value;this[Fe]({filter:s}),this[Se]=!1})),e.filter||e.currentIndex){const{filter:e,currentIndex:t}=this[We];if(""===e||-1===t){const t=this[we].list;"filter"in t&&(t.filter=e)}}}[He](e,t){const s=super[He](e,t);return t.opened&&!e.opened&&Object.assign(s,{filter:""}),s}};function Ar(e){return class extends e{get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid gray;
            box-sizing: border-box;
          }

          [part~="input"] {
            background: transparent;
            border-color: transparent;
          }
        </style>
      `),e}}}class Dr extends(Ar(fr)){}const jr=Dr;class Fr extends(Ar(pr)){}const Mr=Fr;class Rr extends(function(e){return class extends e{get[Je](){const e=super[Je];return e.content.append(A`
          <style>
            :host {
              border: 1px solid gray;
              box-sizing: border-box;
            }

            ::slotted(*),
            #slot > * {
              padding: 0.25em;
            }

            ::slotted([selected]),
            #slot > [selected] {
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
        `),e}}}(kr)){}const Br=Rr;class Wr extends(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{inputPartType:Mr,popupPartType:Qn,popupTogglePartType:Xn})}[ke](e){if(super[ke]&&super[ke](e),e.inputPartType){const e=this[we].input,t="inner"in e?e.inner:e;Object.assign(t.style,{outline:"none"})}if(e.calculatedPopupPosition){const{calculatedPopupPosition:e,opened:t}=this[We],s="10px",n="below"===e?`polygon(0px 0px, 100% 0px, 100% -${s}, calc(100% + ${s}) -${s}, calc(100% + ${s}) calc(100% + ${s}), -${s} calc(100% + ${s}), -${s} -${s}, 0px -${s})`:`polygon(-${s} -${s}, calc(100% + ${s}) -${s}, calc(100% + ${s}) calc(100% + ${s}), 100% calc(100% + ${s}), 100% 100%, 0px 100%, 0px calc(100% + ${s}), -${s} calc(100% + ${s}))`;this[we].popup.style.clipPath=t?n:""}}get[Je](){const e=super[Je];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid gray;
            box-sizing: border-box;
          }

          [part~="input"] {
            background: transparent;
            border: none;
          }
        </style>
      `),e}}}(Or)){get[ee](){return Object.assign(super[ee],{inputPartType:jr,listPartType:Br})}}const Hr=Wr;customElements.define("elix-filter-combo-box",class extends Hr{});const Nr=rs(Zs(dr(vt.wrap("textarea")))),zr=class extends Nr{attributeChangedCallback(e,t,s){"minimum-rows"===e?this.minimumRows=Number(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee],{minimumRows:1,value:null,valueTracksContent:!0})}get minimumRows(){return this[We].minimumRows}set minimumRows(e){isNaN(e)||this[Fe]({minimumRows:e})}[ke](e){super[ke](e),this[ie]&&this[we].inner.addEventListener("input",(()=>{this[Se]=!0,this[Fe]({valueTracksContent:!1});const e=this[we].inner;this[Fe]({value:e.value}),this[Se]=!1}));const{copyStyle:t,lineHeight:s,minimumRows:n,value:i}=this[We];if(e.copyStyle&&Object.assign(this[we].copyContainer.style,t),e.lineHeight||e.minimumRows&&null!=s){const e=n*s;this[we].copyContainer.style.minHeight=e+"px"}e.value&&(this[we].inner.value=i,this[we].textCopy.textContent=i)}[Ae](e){if(super[Ae](e),this[ie]){const e=getComputedStyle(this[we].inner),t=this[we].extraSpace.clientHeight;this[Fe]({copyStyle:{"border-bottom-style":e.borderBottomStyle,"border-bottom-width":e.borderBottomWidth,"border-left-style":e.borderLeftStyle,"border-left-width":e.borderLeftWidth,"border-right-style":e.borderRightStyle,"border-right-width":e.borderRightWidth,"border-top-style":e.borderTopStyle,"border-top-width":e.borderTopWidth,"padding-bottom":e.paddingBottom,"padding-left":e.paddingLeft,"padding-right":e.paddingRight,"padding-top":e.paddingTop},lineHeight:t})}if(e.value&&this[Se]){const{value:e}=this[We],t=new CustomEvent("value-changed",{bubbles:!0,detail:{value:e}});this.dispatchEvent(t);const s=new CustomEvent("input",{bubbles:!0,detail:{value:e}});this.dispatchEvent(s)}}[He](e,t){const s=super[He](e,t);if((t.content||t.valueTracksContent)&&e.valueTracksContent){const t=function(e){if(null===e)return"";const t=[...e].map((e=>e.textContent));return t.join("").trim().replace(/&amp;/g,"&").replace(/&lt;/g,"<").replace(/&gt;/g,">").replace(/&quot;/g,'"').replace(/&#039;/g,"'")}(e.content);Object.assign(s,{value:t})}return s}get[Je](){return D.html`
      <style>
        :host {
          display: block;
        }

        #autoSizeContainer {
          position: relative;
        }

        [part~="textarea"],
        #copyContainer {
          font: inherit;
          margin: 0;
        }

        [part~="textarea"] {
          box-sizing: border-box;
          height: 100%;
          overflow: hidden;
          position: absolute;
          resize: none;
          top: 0;
          width: 100%;
        }

        #copyContainer {
          box-sizing: content-box;
          visibility: hidden;
          white-space: pre-wrap;
          word-wrap: break-word;
        }

        #extraSpace {
          display: inline-block;
          width: 0;
        }
      </style>
      <div id="autoSizeContainer">
        <textarea id="inner" part="inner textarea"></textarea>
        <div id="copyContainer"><span id="textCopy"></span><span id="extraSpace">&nbsp;</span></div>
      </div>
      <div hidden>
        <slot></slot>
      </div>
    `}get value(){return this[We].value}set value(e){this[Fe]({value:String(e),valueTracksContent:!1})}},Yr=class extends zr{};customElements.define("elix-auto-size-textarea",class extends Yr{})})();