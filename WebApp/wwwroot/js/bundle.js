(()=>{"use strict";const e=Symbol("defaultState"),t=Symbol("delegatesFocus"),s=Symbol("firstRender"),n=Symbol("focusTarget"),i=Symbol("hasDynamicTemplate"),r=Symbol("ids"),o=Symbol("nativeInternals"),a=Symbol("raiseChangeEvents"),l=Symbol("render"),c=Symbol("renderChanges"),u=Symbol("rendered"),d=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),m=Symbol("shadowRootMode"),g=Symbol("state"),f=Symbol("stateEffects"),b=Symbol("template"),y=Symbol("mousedownListener");function w(e,t){return"boolean"==typeof t?t:"string"==typeof t&&(""===t||e.toLowerCase()===t.toLowerCase())}function v(e){for(const t of I(e)){const e=t[n]||t,s=e;if(e instanceof HTMLElement&&e.tabIndex>=0&&!s.disabled&&!(e instanceof HTMLSlotElement))return e}return null}function x(e,t){let s=t;for(;s;){const t=s.assignedSlot||s.parentNode||s.host;if(t===e)return!0;s=t}return!1}function T(e){const t=O(e,(e=>e instanceof HTMLElement&&e.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&e.tabIndex>=0)),{value:s}=t.next();return s instanceof HTMLElement?s:null}function P(e,t){e[y]&&e.removeEventListener("mousedown",e[y]),t&&(e[y]=e=>{if(0!==e.button)return;const s=v(t[n]||t);s&&(s.focus(),e.preventDefault())},e.addEventListener("mousedown",e[y]))}function E(e,t){return Array.prototype.findIndex.call(e,(e=>e===t||x(e,t)))}function C(e,t){const s=t.composedPath()[0];return e===s||x(e,s)}function*I(e){e&&(yield e,yield*function*(e){let t=e;for(;t=t instanceof HTMLElement&&t.assignedSlot?t.assignedSlot:t instanceof ShadowRoot?t.host:t.parentNode,t;)yield t}(e))}function S(e,t,s){e.toggleAttribute(t,s),e[o]&&e[o].states&&e[o].states.toggle(t,s)}const k={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function L(e,t){const s=[...t],n=e.childNodes.length,i=s.length,r=Math.max(n,i);for(let t=0;t<r;t++){const r=e.childNodes[t],o=s[t];t>=n?e.append(o):t>=i?e.removeChild(e.childNodes[i]):r!==o&&(s.indexOf(r,t)>=t?e.insertBefore(o,r):e.replaceChild(o,r))}}function*O(e,t){let s;if(t(e)&&(yield e),e instanceof HTMLElement&&e.shadowRoot)s=e.shadowRoot.children;else{const t=e instanceof HTMLSlotElement?e.assignedNodes({flatten:!0}):[];s=t.length>0?t:e.childNodes}if(s)for(let e=0;e<s.length;e++)yield*O(s[e],t)}const A=(e,...t)=>D.html(e,...t).content,D={html(e,...t){const s=document.createElement("template");return s.innerHTML=String.raw(e,...t),s}},M={tabindex:"tabIndex"},F={tabIndex:"tabindex"};function R(e){if(e===HTMLElement)return[];const t=Object.getPrototypeOf(e.prototype).constructor;let s=t.observedAttributes;s||(s=R(t));const n=Object.getOwnPropertyNames(e.prototype).filter((t=>{const s=Object.getOwnPropertyDescriptor(e.prototype,t);return s&&"function"==typeof s.set})).map((e=>function(e){let t=F[e];if(!t){const s=/([A-Z])/g;t=e.replace(s,"-$1").toLowerCase(),F[e]=t}return t}(e))).filter((e=>s.indexOf(e)<0));return s.concat(n)}const j=Symbol("state"),B=Symbol("raiseChangeEventsInNextRender"),H=Symbol("changedSinceLastRender");function W(e,t){const s={};for(const r in t)n=t[r],i=e[r],(n instanceof Date&&i instanceof Date?n.getTime()===i.getTime():n===i)||(s[r]=!0);var n,i;return s}const N=new Map,z=Symbol("shadowIdProxy"),Y=Symbol("proxyElement"),U={get(e,t){const s=e[Y][p];return s&&"string"==typeof t?s.getElementById(t):null}};function V(e){let t=e[i]?void 0:N.get(e.constructor);if(void 0===t&&(t=e[b],t)){if(!(t instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${e.constructor.name} must return an HTMLTemplateElement.`;e[i]||N.set(e.constructor,t)}return t}const G=function(e){return class extends e{attributeChangedCallback(e,t,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(e,t,s),s!==t&&!this[d]){const t=function(e){let t=M[e];if(!t){const s=/-([a-z])/g;t=e.replace(s,(e=>e[1].toUpperCase())),M[e]=t}return t}(e);if(t in this){const n=k[e]?w(e,s):s;this[t]=n}}}static get observedAttributes(){return R(this)}}}(function(t){class n extends t{constructor(){super(),this[s]=void 0,this[a]=!1,this[H]=null,this[h](this[e])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[c]()}get[e](){return super[e]||{}}[l](e){super[l]&&super[l](e)}[c](){void 0===this[s]&&(this[s]=!0);const e=this[H];if(this[s]||e){const t=this[a];this[a]=this[B],this[d]=!0,this[l](e),this[d]=!1,this[H]=null,this[u](e),this[s]=!1,this[a]=t,this[B]=t}}[u](e){super[u]&&super[u](e)}async[h](e){this[d]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:t,changed:n}=function(e,t){const s=Object.assign({},e[j]),n={};let i=t;for(;;){const t=W(s,i);if(0===Object.keys(t).length)break;Object.assign(s,i),Object.assign(n,t),i=e[f](s,t)}return{state:s,changed:n}}(this,e);if(this[j]&&0===Object.keys(n).length)return;Object.freeze(t),this[j]=t,this[a]&&(this[B]=!0);const i=void 0===this[s]||null!==this[H];this[H]=Object.assign(this[H]||{},n),this.isConnected&&!i&&(await Promise.resolve(),this[c]())}get[g](){return this[j]}[f](e,t){return super[f]?super[f](e,t):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(n.prototype,"state",{get(){return this[g]}}),n}(function(e){return class extends e{get[r](){if(!this[z]){const e={[Y]:this};this[z]=new Proxy(e,U)}return this[z]}[l](e){if(super[l]&&super[l](e),void 0===this[s]||this[s]){const e=V(this);if(e){const s=this.attachShadow({delegatesFocus:this[t],mode:this[m]}),n=document.importNode(e.content,!0);s.append(n),this[p]=s}}}get[m](){return"open"}}}(HTMLElement))),q=new Map;function K(e){if("function"==typeof e){let t;try{t=new e}catch(s){if("TypeError"!==s.name)throw s;!function(e){let t;const s=e.name&&e.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const e=/([A-Z])/g;t=s[0].replace(e,((e,t,s)=>s>0?"-"+t:t)).toLowerCase()}else t="custom-element";let n,i=q.get(t)||0;for(;n=`${t}-${i}`,customElements.get(n);i++);customElements.define(n,e),q.set(t,i+1)}(e),t=new e}return t}return document.createElement(e)}function $(e,t){const s=e.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";return(e instanceof HTMLElement||e instanceof SVGElement)&&(t instanceof HTMLElement||t instanceof SVGElement)&&(Array.prototype.forEach.call(e.attributes,(e=>{t.getAttribute(e.name)||"class"===e.name||"style"===e.name||t.setAttribute(e.name,e.value)})),Array.prototype.forEach.call(e.classList,(e=>{t.classList.add(e)})),Array.prototype.forEach.call(e.style,(s=>{t.style[s]||(t.style[s]=e.style[s])}))),t.append(...e.childNodes),s.replaceChild(t,e),t}function X(e,t){if("function"==typeof t&&e.constructor===t||"string"==typeof t&&e instanceof Element&&e.localName===t)return e;{const s=K(t);return $(e,s),s}}const Z=Symbol("applyElementData"),J=Symbol("checkSize"),Q=Symbol("closestAvailableItemIndex"),_=Symbol("contentSlot"),ee=e,te=Symbol("defaultTabIndex"),se=t,ne=Symbol("effectEndTarget"),ie=s,re=n,oe=Symbol("getItemText"),ae=Symbol("goDown"),le=Symbol("goEnd"),ce=Symbol("goFirst"),ue=Symbol("goLast"),de=Symbol("goLeft"),he=Symbol("goNext"),pe=Symbol("goPrevious"),me=Symbol("goRight"),ge=Symbol("goStart"),fe=Symbol("goToItemWithPrefix"),be=Symbol("goUp"),ye=i,we=r,ve=Symbol("inputDelegate"),xe=Symbol("itemsDelegate"),Te=Symbol("keydown"),Pe=(Symbol("matchText"),Symbol("mouseenter")),Ee=Symbol("mouseleave"),Ce=o,Ie=a,Se=l,ke=c,Le=Symbol("renderDataToElement"),Oe=u,Ae=d,De=Symbol("scrollTarget"),Me=h,Fe=p,Re=m,je=Symbol("startEffect"),Be=g,He=f,We=Symbol("swipeDown"),Ne=Symbol("swipeDownComplete"),ze=Symbol("swipeLeft"),Ye=Symbol("swipeLeftTransitionEnd"),Ue=Symbol("swipeRight"),Ve=Symbol("swipeRightTransitionEnd"),Ge=Symbol("swipeUp"),qe=Symbol("swipeUpComplete"),Ke=Symbol("swipeStart"),$e=Symbol("swipeTarget"),Xe=Symbol("tap"),Ze=b,Je=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:J,closestAvailableItemIndex:Q,contentSlot:_,defaultState:ee,defaultTabIndex:te,delegatesFocus:se,effectEndTarget:ne,firstRender:ie,focusTarget:re,getItemText:oe,goDown:ae,goEnd:le,goFirst:ce,goLast:ue,goLeft:de,goNext:he,goPrevious:pe,goRight:me,goStart:ge,goToItemWithPrefix:fe,goUp:be,hasDynamicTemplate:ye,ids:we,inputDelegate:ve,itemsDelegate:xe,keydown:Te,mouseenter:Pe,mouseleave:Ee,nativeInternals:Ce,event,raiseChangeEvents:Ie,render:Se,renderChanges:ke,renderDataToElement:Le,rendered:Oe,rendering:Ae,scrollTarget:De,setState:Me,shadowRoot:Fe,shadowRootMode:Re,startEffect:je,state:Be,stateEffects:He,swipeDown:We,swipeDownComplete:Ne,swipeLeft:ze,swipeLeftTransitionEnd:Ye,swipeRight:Ue,swipeRightTransitionEnd:Ve,swipeUp:Ge,swipeUpComplete:qe,swipeStart:Ke,swipeTarget:$e,tap:Xe,template:Ze,toggleSelectedFlag:Je}});const Qe=document.createElement("div");Qe.attachShadow({mode:"open",delegatesFocus:!0});const _e=Qe.shadowRoot.delegatesFocus;function et(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{composeFocus:!_e})}[Se](e){super[Se]&&super[Se](e),this[ie]&&this.addEventListener("mousedown",(e=>{if(this[Be].composeFocus&&0===e.button&&e.target instanceof Element){const t=v(e.target);t&&(t.focus(),e.preventDefault())}}))}}}function tt(e){return class extends e{get ariaLabel(){return this[Be].ariaLabel}set ariaLabel(e){this[Be].removingAriaAttribute||this[Me]({ariaLabel:e})}get ariaLabelledby(){return this[Be].ariaLabelledby}set ariaLabelledby(e){this[Be].removingAriaAttribute||this[Me]({ariaLabelledby:e})}get[ee](){return Object.assign(super[ee]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[Se](e){if(super[Se]&&super[Se](e),this[ie]&&this.addEventListener("focus",(()=>{this[Ie]=!0;const e=nt(this,this[Be]);this[Me]({inputLabel:e}),this[Ie]=!1})),e.inputLabel){const{inputLabel:e}=this[Be];e?this[ve].setAttribute("aria-label",e):this[ve].removeAttribute("aria-label")}}[Oe](e){super[Oe]&&super[Oe](e),this[ie]&&(window.requestIdleCallback||setTimeout)((()=>{const e=nt(this,this[Be]);this[Me]({inputLabel:e})}));const{ariaLabel:t,ariaLabelledby:s}=this[Be];e.ariaLabel&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",t),this[Me]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),e.ariaLabelledby&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Me]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),e.removingAriaAttribute&&this[Be].removingAriaAttribute&&this[Me]({removingAriaAttribute:!1})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.ariaLabel&&e.ariaLabel||t.selectedText&&e.ariaLabelledby&&this.matches(":focus-within")){const t=nt(this,e);Object.assign(s,{inputLabel:t})}return s}}}function st(e){if("selectedText"in e)return e.selectedText;if("value"in e&&"options"in e){const t=e.value,s=e.options.find((e=>e.value===t));return s?s.innerText:""}return"value"in e?e.value:e.innerText}function nt(e,t){const{ariaLabel:s,ariaLabelledby:n}=t,i=e.isConnected?e.getRootNode():null;let r=null;if(n&&i)r=n.split(" ").map((s=>{const n=i.getElementById(s);return n?n===e&&null!==t.value?t.selectedText:st(n):""})).join(" ");else if(s)r=s;else if(i){const t=e.id;if(t){const e=i.querySelector(`[for="${t}"]`);e instanceof HTMLElement&&(r=st(e))}if(null===r){const t=e.closest("label");t&&(r=st(t))}}return r&&(r=r.trim()),r}let it=!1;const rt=Symbol("focusVisibleChangedListener");function ot(e){return class extends e{constructor(){super(),this.addEventListener("focusout",(e=>{Promise.resolve().then((()=>{const t=e.relatedTarget||document.activeElement,s=this===t,n=x(this,t);!s&&!n&&(this[Me]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[rt]),this[rt]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[Be].focusVisible!==it&&this[Me]({focusVisible:it}),this[rt]||(this[rt]=()=>{this[Me]({focusVisible:it})},document.addEventListener("focusvisiblechange",this[rt]))}))}))}get[ee](){return Object.assign(super[ee]||{},{focusVisible:!1})}[Se](e){if(super[Se]&&super[Se](e),e.focusVisible){const{focusVisible:e}=this[Be];this.toggleAttribute("focus-visible",e)}}get[Ze](){const e=super[Ze]||D.html``;return e.content.append(A`
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
      `),e}}}function at(e){if(it!==e){it=e;const t=new CustomEvent("focus-visible-changed",{detail:{focusVisible:it}});document.dispatchEvent(t);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:it}});document.dispatchEvent(s)}}function lt(e){return class extends e{get[se](){return!0}focus(e){const t=this[re];t&&t.focus(e)}get[re](){return T(this[Fe])}}}window.addEventListener("keydown",(()=>{at(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{at(!1)}),{capture:!0});const ct=Symbol("extends"),ut=Symbol("delegatedPropertySetters"),dt={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},ht={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},pt=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],mt={abort:!0,change:!0,reset:!0},gt=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],ft=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],bt=lt(G);class yt extends bt{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if(ft.indexOf(e)>=0){const t=Object.assign({},this[Be].innerAttributes,{[e]:s});this[Me]({innerAttributes:t})}else super.attributeChangedCallback(e,t,s)}blur(){this.inner.blur()}get[ee](){return Object.assign(super[ee],{innerAttributes:{}})}get[te](){return dt[this.extends]?0:-1}get extends(){return this.constructor[ct]}get inner(){const e=this[we]&&this[we].inner;return e||console.warn("Attempted to get an inner standard element before it was instantiated."),e}getInnerProperty(e){return this[Be][e]||this[Fe]&&this.inner[e]}static get observedAttributes(){return[...super.observedAttributes,...ft]}[Se](e){super[Se](e);const t=this.inner;if(this[ie]&&((ht[this.extends]||[]).forEach((e=>{t.addEventListener(e,(()=>{const t=new Event(e,{bubbles:mt[e]||!1});this.dispatchEvent(t)}))})),"disabled"in t&&pt.forEach((e=>{this.addEventListener(e,(e=>{t.disabled&&e.stopImmediatePropagation()}))}))),e.tabIndex&&(t.tabIndex=this[Be].tabIndex),e.innerAttributes){const{innerAttributes:e}=this[Be];for(const s in e)wt(t,s,e[s])}this.constructor[ut].forEach((s=>{if(e[s]){const e=this[Be][s];("selectionEnd"===s||"selectionStart"===s)&&null===e||(t[s]=e)}}))}[Oe](e){if(super[Oe](e),e.disabled){const{disabled:e}=this[Be];void 0!==e&&S(this,"disabled",e)}}setInnerProperty(e,t){this[Be][e]!==t&&this[Me]({[e]:t})}get[Ze](){const e=gt.includes(this.extends)?"block":"inline-block";return D.html`
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
    `}static wrap(e){class t extends yt{}t[ct]=e;const s=document.createElement(e);return function(e,t){const s=Object.getOwnPropertyNames(t);e[ut]=[],s.forEach((s=>{const n=Object.getOwnPropertyDescriptor(t,s);if(!n)return;const i=function(e,t){if("function"==typeof t.value){if("constructor"!==e)return function(e,t){return{configurable:t.configurable,enumerable:t.enumerable,value:function(...t){this.inner[e](...t)},writable:t.writable}}(e,t)}else if("function"==typeof t.get||"function"==typeof t.set)return function(e,t){const s={configurable:t.configurable,enumerable:t.enumerable};return t.get&&(s.get=function(){return this.getInnerProperty(e)}),t.set&&(s.set=function(t){this.setInnerProperty(e,t)}),t.writable&&(s.writable=t.writable),s}(e,t);return null}(s,n);i&&(Object.defineProperty(e.prototype,s,i),i.set&&e[ut].push(s))}))}(t,Object.getPrototypeOf(s)),t}}function wt(e,t,s){k[t]?"string"==typeof s?e.setAttribute(t,""):null===s&&e.removeAttribute(t):null!=s?e.setAttribute(t,s.toString()):e.removeAttribute(t)}const vt=et(tt(ot(yt.wrap("button")))),xt=class extends vt{get[ee](){return Object.assign(super[ee],{role:"button"})}get[ve](){return this[we].inner}[Xe](){const e=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(e)}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},Tt=Symbol("wrap");function Pt(e){return class extends e{get arrowButtonOverlap(){return this[Be].arrowButtonOverlap}set arrowButtonOverlap(e){this[Me]({arrowButtonOverlap:e})}get arrowButtonPartType(){return this[Be].arrowButtonPartType}set arrowButtonPartType(e){this[Me]({arrowButtonPartType:e})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[pe]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[he]()}attributeChangedCallback(e,t,s){"arrow-button-overlap"===e?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===e?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{arrowButtonOverlap:!0,arrowButtonPartType:xt,orientation:"horizontal",showArrowButtons:!0})}[Se](e){if(e.arrowButtonPartType){const e=this[we].arrowButtonPrevious;e instanceof HTMLElement&&P(e,null);const t=this[we].arrowButtonNext;t instanceof HTMLElement&&P(t,null)}if(super[Se]&&super[Se](e),Ct(this[Fe],this[Be],e),e.arrowButtonPartType){const e=this,t=this[we].arrowButtonPrevious;t instanceof HTMLElement&&P(t,e);const s=Et(this,(()=>this.arrowButtonPrevious()));t.addEventListener("mousedown",s);const n=this[we].arrowButtonNext;n instanceof HTMLElement&&P(n,e);const i=Et(this,(()=>this.arrowButtonNext()));n.addEventListener("mousedown",i)}const{arrowButtonOverlap:t,canGoNext:s,canGoPrevious:n,orientation:i,rightToLeft:r}=this[Be],o="vertical"===i,a=this[we].arrowButtonPrevious,l=this[we].arrowButtonNext;if(e.arrowButtonOverlap||e.orientation||e.rightToLeft){this[we].arrowDirection.style.flexDirection=o?"column":"row";const e={bottom:null,left:null,right:null,top:null};let s,n;t?Object.assign(e,{position:"absolute","z-index":1}):Object.assign(e,{position:null,"z-index":null}),t&&(o?(Object.assign(e,{left:0,right:0}),s={top:0},n={bottom:0}):(Object.assign(e,{bottom:0,top:0}),r?(s={right:0},n={left:0}):(s={left:0},n={right:0}))),Object.assign(a.style,e,s),Object.assign(l.style,e,n)}if(e.canGoNext&&null!==s&&(l.disabled=!s),e.canGoPrevious&&null!==n&&(a.disabled=!n),e.showArrowButtons){const e=this[Be].showArrowButtons?null:"none";a.style.display=e,l.style.display=e}}get showArrowButtons(){return this[Be].showArrowButtons}set showArrowButtons(e){this[Me]({showArrowButtons:e})}[Tt](e){const t=A`
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
      `;Ct(t,this[Be]);const s=t.getElementById("arrowDirectionContainer");s&&(e.replaceWith(t),s.append(e))}}}function Et(e,t){return async function(s){0===s.button&&(e[Ie]=!0,t()&&s.stopPropagation(),await Promise.resolve(),e[Ie]=!1)}}function Ct(e,t,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=t,n=e.getElementById("arrowButtonPrevious");n&&X(n,s);const i=e.getElementById("arrowButtonNext");i&&X(i,s)}}Pt.wrap=Tt;const It=Pt,St={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},kt=864e5;function Lt(e,t){const s=e.includes("-ca-")?"":"-ca-gregory",n=e.includes("-nu-")?"":"-nu-latn",i=`${e}${s||n?"-u":""}${s}${n}`;return new Intl.DateTimeFormat(i,t)}function Ot(e,t){return null===e&&null===t||null!==e&&null!==t&&e.getTime()===t.getTime()}function At(e,t){const s=Dt(t);return(e.getDay()-s+7)%7}function Dt(e){const t=Yt(e),s=St.firstDay[t];return void 0!==s?s:St.firstDay["001"]}function Mt(e){const t=Ft(e);return t.setDate(1),t}function Ft(e){const t=new Date(e.getTime());return t.setHours(0),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Rt(e){const t=new Date(e.getTime());return t.setHours(12),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function jt(e,t){const s=Rt(e);return s.setDate(s.getDate()+t),zt(e,s),s}function Bt(e,t){const s=Rt(e);return s.setMonth(e.getMonth()+t),zt(e,s),s}function Ht(){return Ft(new Date)}function Wt(e){const t=Yt(e),s=St.weekendEnd[t];return void 0!==s?s:St.weekendEnd["001"]}function Nt(e){const t=Yt(e),s=St.weekendStart[t];return void 0!==s?s:St.weekendStart["001"]}function zt(e,t){t.setHours(e.getHours()),t.setMinutes(e.getMinutes()),t.setSeconds(e.getSeconds()),t.setMilliseconds(e.getMilliseconds())}function Yt(e){const t=e?e.split("-"):null;return t?t[1]:"001"}function Ut(e){return class extends e{attributeChangedCallback(e,t,s){"date"===e?this.date=new Date(s):super.attributeChangedCallback(e,t,s)}get date(){return this[Be].date}set date(e){Ot(e,this[Be].date)||this[Me]({date:e})}get[ee](){return Object.assign(super[ee]||{},{date:null,locale:navigator.language})}get locale(){return this[Be].locale}set locale(e){this[Me]({locale:e})}[Oe](e){if(super[Oe]&&super[Oe](e),e.date&&this[Ie]){const e=this[Be].date,t=new CustomEvent("date-changed",{bubbles:!0,detail:{date:e}});this.dispatchEvent(t);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:e}});this.dispatchEvent(s)}}}}function Vt(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}get[ee](){return Object.assign(super[ee]||{},{selected:!1})}[Se](e){if(super[Se](e),e.selected){const{selected:e}=this[Be];S(this,"selected",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.selected){const{selected:e}=this[Be],t=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(s)}}get selected(){return this[Be].selected}set selected(e){this[Me]({selected:e})}}}const Gt=Ut(Vt(G)),qt=class extends Gt{get[ee](){return Object.assign(super[ee],{date:Ht(),outsideRange:!1})}[Se](e){super[Se](e);const{date:t}=this[Be];if(e.date){const e=Ht(),s=t.getDay(),n=t.getDate(),i=jt(t,1),r=Math.round(t.getTime()-e.getTime())/kt;S(this,"alternate-month",Math.abs(t.getMonth()-e.getMonth())%2==1),S(this,"first-day-of-month",1===n),S(this,"first-week",n<=7),S(this,"future",t>e),S(this,"last-day-of-month",t.getMonth()!==i.getMonth()),S(this,"past",t<e),S(this,"sunday",0===s),S(this,"monday",1===s),S(this,"tuesday",2===s),S(this,"wednesday",3===s),S(this,"thursday",4===s),S(this,"friday",5===s),S(this,"saturday",6===s),S(this,"today",0===r),this[we].day.textContent=n.toString()}if(e.date||e.locale){const e=t.getDay(),{locale:s}=this[Be],n=e===Nt(s)||e===Wt(s);S(this,"weekday",!n),S(this,"weekend",n)}e.outsideRange&&S(this,"outside-range",this[Be].outsideRange)}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Me]({outsideRange:e})}get[Ze](){return D.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Kt=Vt(xt),$t=Ut(class extends Kt{}),Xt=class extends $t{get[ee](){return Object.assign(super[ee],{date:Ht(),dayPartType:qt,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Me]({outsideRange:e})}[Se](e){if(super[Se](e),e.dayPartType){const{dayPartType:e}=this[Be];X(this[we].day,e)}const t=this[we].day;(e.dayPartType||e.date)&&(t.date=this[Be].date),(e.dayPartType||e.locale)&&(t.locale=this[Be].locale),(e.dayPartType||e.outsideRange)&&(t.outsideRange=this[Be].outsideRange),(e.dayPartType||e.selected)&&(t.selected=this[Be].selected)}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");if(t){const e=K(this[Be].dayPartType);e.id="day",t.replaceWith(e)}return e.content.append(A`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),e}},Zt=class extends G{get[ee](){return Object.assign(super[ee],{format:"short",locale:navigator.language})}get format(){return this[Be].format}set format(e){this[Me]({format:e})}get locale(){return this[Be].locale}set locale(e){this[Me]({locale:e})}[Se](e){if(super[Se](e),e.format||e.locale){const{format:e,locale:t}=this[Be],s=Lt(t,{weekday:e}),n=Dt(t),i=Nt(t),r=Wt(t),o=new Date(2017,0,1),a=this[Fe].querySelectorAll('[part~="day-name"]');for(let e=0;e<=6;e++){const t=(n+e)%7;o.setDate(t+1);const l=t===i||t===r,c=a[e];c.toggleAttribute("weekday",!l),c.toggleAttribute("weekend",l),c.textContent=s.format(o)}}}get[Ze](){return D.html`
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
    `}},Jt=Ut(G),Qt=class extends Jt{attributeChangedCallback(e,t,s){"start-date"===e?this.startDate=new Date(s):super.attributeChangedCallback(e,t,s)}dayElementForDate(e){return(this.days||[]).find((t=>Ot(t.date,e)))}get dayCount(){return this[Be].dayCount}set dayCount(e){this[Me]({dayCount:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get days(){return this[Be].days}get[ee](){const e=Ht();return Object.assign(super[ee],{date:e,dayCount:1,dayPartType:qt,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:e})}[Se](e){if(super[Se](e),e.days&&L(this[we].dayContainer,this[Be].days),e.date||e.locale||e.showSelectedDay){const e=this[Be].showSelectedDay,{date:t}=this[Be],s=t.getDate(),n=t.getMonth(),i=t.getFullYear();(this.days||[]).forEach((t=>{const r=t.date,o=e&&r.getDate()===s&&r.getMonth()===n&&r.getFullYear()===i;t.toggleAttribute("selected",o)}))}if(e.dayCount||e.startDate){const{dayCount:e,startDate:t}=this[Be],s=jt(t,e);(this[Be].days||[]).forEach((e=>{if("outsideRange"in e){const n=e.date.getTime(),i=n<t.getTime()||n>=s.getTime();e.outsideRange=i}}))}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Me]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Me]({showSelectedDay:e})}get startDate(){return this[Be].startDate}set startDate(e){Ot(this[Be].startDate,e)||this[Me]({startDate:e})}[He](e,t){const s=super[He](e,t);if(t.dayCount||t.dayPartType||t.locale||t.showCompleteWeeks||t.startDate){const n=function(e,t){const{dayCount:s,dayPartType:n,locale:i,showCompleteWeeks:r,startDate:o}=e,a=r?function(e,t){return Ft(jt(e,-At(e,t)))}(o,i):Ft(o);let l;if(r){c=a,u=function(e,t){return Ft(jt(e,6-At(e,t)))}(jt(o,s-1),i),l=Math.round((u.getTime()-c.getTime())/kt)+1}else l=s;var c,u;let d=e.days?e.days.slice():[],h=a;for(let e=0;e<l;e++){const s=t||e>=d.length,r=s?K(n):d[e];r.date=new Date(h.getTime()),r.locale=i,"part"in r&&(r.part="day"),r.style.gridColumnStart="",s&&(d[e]=r),h=jt(h,1)}l<d.length&&(d=d.slice(0,l));const p=d[0];if(p&&!r){const t=At(p.date,e.locale);p.style.gridColumnStart=t+1}return Object.freeze(d),d}(e,t.dayPartType);Object.assign(s,{days:n})}return s}get[Ze](){return D.html`
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
    `}},_t=Ut(G),es=class extends _t{get[ee](){return Object.assign(super[ee],{date:Ht(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Me]({monthFormat:e})}[Se](e){if(super[Se](e),e.date||e.locale||e.monthFormat||e.yearFormat){const{date:e,locale:t,monthFormat:s,yearFormat:n}=this[Be],i={};s&&(i.month=s),n&&(i.year=n);const r=Lt(t,i);this[we].formatted.textContent=r.format(e)}}get[Ze](){return D.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Me]({yearFormat:e})}},ts=Ut(G);function ss(e,t,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=t,n=e.getElementById("dayNamesHeader");n&&X(n,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=t,n=e.getElementById("monthYearHeader");n&&X(n,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=t,n=e.getElementById("monthDays");n&&X(n,s)}}function ns(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}checkValidity(){return this[Ce].checkValidity()}get[ee](){return Object.assign(super[ee]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[Ce]}static get formAssociated(){return!0}get form(){return this[Ce].form}get name(){return this[Be]?this[Be].name:""}set name(t){"name"in e.prototype&&(super.name=t),this[Me]({name:t})}[Se](e){if(super[Se]&&super[Se](e),e.name&&this.setAttribute("name",this[Be].name),this[Ce]&&this[Ce].setValidity&&(e.valid||e.validationMessage)){const{valid:e,validationMessage:t}=this[Be];e?this[Ce].setValidity({}):this[Ce].setValidity({customError:!0},t)}}[Oe](e){super[Oe]&&super[Oe](e),e.value&&this[Ce]&&this[Ce].setFormValue(this[Be].value,this[Be])}reportValidity(){return this[Ce].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[Be].validationMessage}get validity(){return this[Ce].validity}get willValidate(){return this[Ce].willValidate}}}function is(e){return class extends e{[ae](){if(super[ae])return super[ae]()}[le](){if(super[le])return super[le]()}[de](){if(super[de])return super[de]()}[me](){if(super[me])return super[me]()}[ge](){if(super[ge])return super[ge]()}[be](){if(super[be])return super[be]()}[Te](e){let t=!1;const s=this[Be].orientation||"both",n="horizontal"===s||"both"===s,i="vertical"===s||"both"===s;switch(e.key){case"ArrowDown":i&&(t=e.altKey?this[le]():this[ae]());break;case"ArrowLeft":!n||e.metaKey||e.altKey||(t=this[de]());break;case"ArrowRight":!n||e.metaKey||e.altKey||(t=this[me]());break;case"ArrowUp":i&&(t=e.altKey?this[ge]():this[be]());break;case"End":t=this[le]();break;case"Home":t=this[ge]()}return t||super[Te]&&super[Te](e)||!1}}}function rs(e){return class extends e{constructor(){super(),this.addEventListener("keydown",(async e=>{this[Ie]=!0,this[Be].focusVisible||this[Me]({focusVisible:!0}),this[Te](e)&&(e.preventDefault(),e.stopImmediatePropagation()),await Promise.resolve(),this[Ie]=!1}))}attributeChangedCallback(e,t,s){if("tabindex"===e){let e;null===s?e=-1:(e=Number(s),isNaN(e)&&(e=this[te]?this[te]:0)),this.tabIndex=e}else super.attributeChangedCallback(e,t,s)}get[ee](){const e=this[se]?-1:0;return Object.assign(super[ee]||{},{tabIndex:e})}[Te](e){return!!super[Te]&&super[Te](e)}[Se](e){super[Se]&&super[Se](e),e.tabIndex&&(this.tabIndex=this[Be].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(e){super.tabIndex!==e&&(super.tabIndex=e),this[Ae]||this[Me]({tabIndex:e})}}}function os(e){return class extends e{connectedCallback(){const e="rtl"===getComputedStyle(this).direction;this[Me]({rightToLeft:e}),super.connectedCallback()}}}const as=It(Ut(ot(ns(is(rs(os(class extends ts{dayElementForDate(e){const t=this[we].monthDays;return t&&"dayElementForDate"in t&&t.dayElementForDate(e)}get dayNamesHeaderPartType(){return this[Be].dayNamesHeaderPartType}set dayNamesHeaderPartType(e){this[Me]({dayNamesHeaderPartType:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Me]({dayPartType:e})}get days(){return this[Fe]?this[we].monthDays.days:[]}get daysOfWeekFormat(){return this[Be].daysOfWeekFormat}set daysOfWeekFormat(e){this[Me]({daysOfWeekFormat:e})}get[ee](){return Object.assign(super[ee],{date:Ht(),dayNamesHeaderPartType:Zt,dayPartType:qt,daysOfWeekFormat:"short",monthDaysPartType:Qt,monthFormat:"long",monthYearHeaderPartType:es,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Me]({monthFormat:e})}get monthDaysPartType(){return this[Be].monthDaysPartType}set monthDaysPartType(e){this[Me]({monthDaysPartType:e})}get monthYearHeaderPartType(){return this[Be].monthYearHeaderPartType}set monthYearHeaderPartType(e){this[Me]({monthYearHeaderPartType:e})}[Se](e){if(super[Se](e),ss(this[Fe],this[Be],e),(e.dayPartType||e.monthDaysPartType)&&(this[we].monthDays.dayPartType=this[Be].dayPartType),e.locale||e.monthDaysPartType||e.monthYearHeaderPartType||e.dayNamesHeaderPartType){const e=this[Be].locale;this[we].monthDays.locale=e,this[we].monthYearHeader.locale=e,this[we].dayNamesHeader.locale=e}if(e.date||e.monthDaysPartType){const{date:e}=this[Be];if(e){const t=Mt(e),s=function(e){const t=Mt(e);return t.setMonth(t.getMonth()+1),t.setDate(t.getDate()-1),t}(e).getDate();Object.assign(this[we].monthDays,{date:e,dayCount:s,startDate:t}),this[we].monthYearHeader.date=Mt(e)}}if(e.daysOfWeekFormat||e.dayNamesHeaderPartType){const{daysOfWeekFormat:e}=this[Be];this[we].dayNamesHeader.format=e}if(e.showCompleteWeeks||e.monthDaysPartType){const{showCompleteWeeks:e}=this[Be];this[we].monthDays.showCompleteWeeks=e}if(e.showSelectedDay||e.monthDaysPartType){const{showSelectedDay:e}=this[Be];this[we].monthDays.showSelectedDay=e}if(e.monthFormat||e.monthYearHeaderPartType){const{monthFormat:e}=this[Be];this[we].monthYearHeader.monthFormat=e}if(e.yearFormat||e.monthYearHeaderPartType){const{yearFormat:e}=this[Be];this[we].monthYearHeader.yearFormat=e}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Me]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Me]({showSelectedDay:e})}get[Ze](){const e=D.html`
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
    `;return ss(e.content,this[Be]),e}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Me]({yearFormat:e})}}))))))),ls=class extends as{constructor(){super(),this.addEventListener("mousedown",(e=>{if(0!==e.button)return;this[Ie]=!0;const t=e.composedPath()[0];if(t instanceof Node){const e=this.days,s=e[E(e,t)];s&&(this.date=s.date)}this[Ie]=!1})),P(this,this)}arrowButtonNext(){const e=this[Be].date||Ht();return this[Me]({date:Bt(e,1)}),!0}arrowButtonPrevious(){const e=this[Be].date||Ht();return this[Me]({date:Bt(e,-1)}),!0}get[ee](){return Object.assign(super[ee],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Ht(),dayPartType:Xt,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[Te](e){let t=!1;switch(e.key){case"Home":this[Me]({date:Ht()}),t=!0;break;case"PageDown":this[Me]({date:Bt(this[Be].date,1)}),t=!0;break;case"PageUp":this[Me]({date:Bt(this[Be].date,-1)}),t=!0}return t||super[Te]&&super[Te](e)}[ae](){return super[ae]&&super[ae](),this[Me]({date:jt(this[Be].date,7)}),!0}[de](){return super[de]&&super[de](),this[Me]({date:jt(this[Be].date,-1)}),!0}[me](){return super[me]&&super[me](),this[Me]({date:jt(this[Be].date,1)}),!0}[be](){return super[be]&&super[be](),this[Me]({date:jt(this[Be].date,-7)}),!0}[He](e,t){const s=super[He](e,t);return t.date&&Object.assign(s,{value:e.date?e.date.toString():""}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("#monthYearHeader");this[It.wrap](t);const s=D.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return e.content.append(s.content),e}get value(){return this.date}set value(e){this.date=e}},cs=new Set;function us(e){return class extends e{attributeChangedCallback(e,t,s){if("dark"===e){const t=w(e,s);this.dark!==t&&(this.dark=t)}else super.attributeChangedCallback(e,t,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),cs.delete(this)}get dark(){return this[Be].dark}set dark(e){this[Me]({dark:e})}get[ee](){return Object.assign(super[ee]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[Be].detectDarkMode}set detectDarkMode(e){"auto"!==e&&"off"!==e||this[Me]({detectDarkMode:e})}[Se](e){if(super[Se]&&super[Se](e),e.dark){const{dark:e}=this[Be];S(this,"dark",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.detectDarkMode){const{detectDarkMode:e}=this[Be];"auto"===e?(cs.add(this),ds(this)):cs.delete(this)}}}}function ds(e){const t=function(e){const t=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(e);return t?{r:t[1],g:t[2],b:t[3]}:null}(hs(e));if(t){const s=function(e){const t=e.r/255,s=e.g/255,n=e.b/255,i=Math.max(t,s,n),r=Math.min(t,s,n);let o=0,a=0,l=(i+r)/2;const c=i-r;if(0!==c){switch(a=l>.5?c/(2-c):c/(i+r),i){case t:o=(s-n)/c+(s<n?6:0);break;case s:o=(n-t)/c+2;break;case n:o=(t-s)/c+4}o/=6}return{h:o,s:a,l}}(t).l<.5;e[Me]({dark:s})}}function hs(e){const t="rgb(255,255,255)";if(e instanceof Document)return t;const s=getComputedStyle(e).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(e.assignedSlot)return hs(e.assignedSlot);const n=e.parentNode;return n instanceof ShadowRoot?hs(n.host):n instanceof Element?hs(n):t}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{cs.forEach((e=>{ds(e)}))}));class ps extends(function(e){return class extends e{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}}}(xt)){}const ms=ps,gs=us(ms),fs=class extends gs{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},bs=class extends qt{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},ys=class extends Xt{get[ee](){return Object.assign(super[ee],{dayPartType:bs})}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},ws=class extends Zt{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},vs=class extends es{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),e}};class xs extends(us(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{arrowButtonPartType:fs})}[Se](e){if(super[Se](e),e.orientation||e.rightToLeft){const{orientation:e,rightToLeft:t}=this[Be],s="vertical"===e?"rotate(90deg)":t?"rotateZ(180deg)":"";this[we].arrowIconPrevious&&(this[we].arrowIconPrevious.style.transform=s),this[we].arrowIconNext&&(this[we].arrowIconNext.style.transform=s)}if(e.dark){const{dark:e}=this[Be],t=this[we].arrowButtonPrevious,s=this[we].arrowButtonNext;"dark"in t&&(t.dark=e),"dark"in s&&(s.dark=e)}}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="arrowButtonPrevious"]');t&&t.append(A`
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
          `),e}}}(ls))){get[ee](){return Object.assign(super[ee],{dayNamesHeaderPartType:ws,dayPartType:ys,monthYearHeaderPartType:vs})}}const Ts=xs;customElements.define("elix-calendar-month-navigator",class extends Ts{}),Symbol("generatedId");const Ps={a:"link",article:"region",button:"button",h1:"sectionhead",h2:"sectionhead",h3:"sectionhead",h4:"sectionhead",h5:"sectionhead",h6:"sectionhead",hr:"sectionhead",iframe:"region",link:"link",menu:"menu",ol:"list",option:"option",output:"liveregion",progress:"progressbar",select:"select",table:"table",td:"td",textarea:"textbox",th:"th",ul:"list"};function Es(e){return class extends e{attributeChangedCallback(e,t,s){if("current-index"===e)this.currentIndex=Number(s);else if("current-item-required"===e){const t=w(e,s);this.currentItemRequired!==t&&(this.currentItemRequired=t)}else if("cursor-operations-wrap"===e){const t=w(e,s);this.cursorOperationsWrap!==t&&(this.cursorOperationsWrap=t)}else super.attributeChangedCallback(e,t,s)}get currentIndex(){const{items:e,currentIndex:t}=this[Be];return e&&e.length>0?t:-1}set currentIndex(e){isNaN(e)||this[Me]({currentIndex:e})}get currentItem(){const{items:e,currentIndex:t}=this[Be];return e&&e[t]}set currentItem(e){const{items:t}=this[Be];if(!t)return;const s=t.indexOf(e);s>=0&&this[Me]({currentIndex:s})}get currentItemRequired(){return this[Be].currentItemRequired}set currentItemRequired(e){this[Me]({currentItemRequired:e})}get cursorOperationsWrap(){return this[Be].cursorOperationsWrap}set cursorOperationsWrap(e){this[Me]({cursorOperationsWrap:e})}goFirst(){return super.goFirst&&super.goFirst(),this[ce]()}goLast(){return super.goLast&&super.goLast(),this[ue]()}goNext(){return super.goNext&&super.goNext(),this[he]()}goPrevious(){return super.goPrevious&&super.goPrevious(),this[pe]()}[Oe](e){if(super[Oe]&&super[Oe](e),e.currentIndex&&this[Ie]){const{currentIndex:e}=this[Be],t=new CustomEvent("current-index-changed",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("currentindexchange",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(s)}}}}function Cs(e,t,s){if(!(e instanceof Node))return!1;for(const n of I(e))if(n instanceof HTMLElement){const e=getComputedStyle(n),i="vertical"===t;if(i&&("scroll"===e.overflowY||"auto"===e.overflowY)||!i&&("scroll"===e.overflowX||"auto"===e.overflowX)){const e=i?"scrollTop":"scrollLeft";if(!s&&n[e]>0)return!0;const t=i?"clientHeight":"clientWidth",r=n[i?"scrollHeight":"scrollWidth"]-n[t];if(s&&n[e]<r)return!0}}return!1}function Is(e){const t=e[Fe],s=t&&t.querySelector("slot:not([name])");return s&&s.parentNode instanceof Element&&function(e){for(const t of I(e))if(t instanceof HTMLElement&&Ss(t))return t;return null}(s.parentNode)||e}function Ss(e){const t=getComputedStyle(e),s=t.overflowX,n=t.overflowY;return"scroll"===s||"auto"===s||"scroll"===n||"auto"===n}function ks(e){return class extends e{[Oe](e){super[Oe]&&super[Oe](e),e.currentItem&&this.scrollCurrentItemIntoView()}scrollCurrentItemIntoView(){super.scrollCurrentItemIntoView&&super.scrollCurrentItemIntoView();const{currentItem:e,items:t}=this[Be];if(!e||!t)return;const s=this[De].getBoundingClientRect(),n=e.getBoundingClientRect(),i=n.bottom-s.bottom,r=n.left-s.left,o=n.right-s.right,a=n.top-s.top,l=this[Be].orientation||"both";"horizontal"!==l&&"both"!==l||(o>0?this[De].scrollLeft+=o:r<0&&(this[De].scrollLeft+=Math.ceil(r))),"vertical"!==l&&"both"!==l||(i>0?this[De].scrollTop+=i:a<0&&(this[De].scrollTop+=Math.ceil(a)))}get[De](){return super[De]||Is(this)}}}function Ls(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{canGoDown:null,canGoLeft:null,canGoRight:null,canGoUp:null})}[ae](){return super[ae]&&super[ae](),this[he]()}[le](){return super[le]&&super[le](),this[ue]()}[de](){return super[de]&&super[de](),this[Be]&&this[Be].rightToLeft?this[he]():this[pe]()}[me](){return super[me]&&super[me](),this[Be]&&this[Be].rightToLeft?this[pe]():this[he]()}[ge](){return super[ge]&&super[ge](),this[ce]()}[be](){return super[be]&&super[be](),this[pe]()}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.canGoNext||t.canGoPrevious||t.languageDirection||t.orientation||t.rightToLeft){const{canGoNext:t,canGoPrevious:n,orientation:i,rightToLeft:r}=e,o="horizontal"===i||"both"===i,a="vertical"===i||"both"===i,l=a&&t,c=!!o&&(r?t:n),u=!!o&&(r?n:t),d=a&&n;Object.assign(s,{canGoDown:l,canGoLeft:c,canGoRight:u,canGoUp:d})}return s}}}function Os(e){return class extends e{get items(){return this[Be]?this[Be].items:null}[Oe](e){if(super[Oe]&&super[Oe](e),!this[ie]&&e.items&&this[Ie]){const e=new CustomEvent("items-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("itemschange",{bubbles:!0});this.dispatchEvent(t)}}}}function As(e){return class extends e{[Q](e,t={}){const s=void 0!==t.direction?t.direction:1,n=void 0!==t.index?t.index:e.currentIndex,i=void 0!==t.wrap?t.wrap:e.cursorOperationsWrap,{items:r}=e,o=r?r.length:0;if(0===o)return-1;if(i){let t=(n%o+o)%o;const i=((t-s)%o+o)%o;for(;t!==i;){if(!e.availableItemFlags||e.availableItemFlags[t])return t;t=((t+s)%o+o)%o}}else for(let t=n;t>=0&&t<o;t+=s)if(!e.availableItemFlags||e.availableItemFlags[t])return t;return-1}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,desiredCurrentIndex:null,currentItem:null,currentItemRequired:!1,cursorOperationsWrap:!1})}[ce](){return super[ce]&&super[ce](),Ds(this,0,1)}[ue](){return super[ue]&&super[ue](),Ds(this,this[Be].items.length-1,-1)}[he](){super[he]&&super[he]();const{currentIndex:e,items:t}=this[Be];return Ds(this,e<0&&t?0:e+1,1)}[pe](){super[pe]&&super[pe]();const{currentIndex:e,items:t}=this[Be];return Ds(this,e<0&&t?t.length-1:e-1,-1)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.availableItemFlags||t.items||t.currentIndex||t.currentItemRequired){const{currentIndex:n,desiredCurrentIndex:i,currentItem:r,currentItemRequired:o,items:a}=e,l=a?a.length:0;let c,u=i;if(t.items&&!t.currentIndex&&r&&l>0&&a[n]!==r){const e=a.indexOf(r);e>=0&&(u=e)}else t.currentIndex&&(n<0&&null!==r||n>=0&&(0===l||a[n]!==r)||null===i)&&(u=n);o&&u<0&&(u=0),u<0?(u=-1,c=-1):0===l?c=-1:(c=Math.max(Math.min(l-1,u),0),c=this[Q](e,{direction:1,index:c,wrap:!1}),c<0&&(c=this[Q](e,{direction:-1,index:c-1,wrap:!1})));const d=a&&a[c]||null;Object.assign(s,{currentIndex:c,desiredCurrentIndex:u,currentItem:d})}return s}}}function Ds(e,t,s){const n=e[Q](e[Be],{direction:s,index:t});if(n<0)return!1;const i=e[Be].currentIndex!==n;return i&&e[Me]({currentIndex:n}),i}const Ms=["applet","basefont","embed","font","frame","frameset","isindex","keygen","link","multicol","nextid","noscript","object","param","script","style","template","noembed"];function Fs(e){return e.getAttribute("aria-label")||e.getAttribute("alt")||e.innerText||e.textContent||""}function Rs(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{texts:null})}[oe](e){return super[oe]?super[oe](e):Fs(e)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items){const{items:t}=e,n=function(e,t){return e?Array.from(e,(e=>t(e))):null}(t,this[oe]);n&&(Object.freeze(n),Object.assign(s,{texts:n}))}return s}}}function js(e){return class extends e{[Te](e){let t=!1;if("horizontal"!==this.orientation)switch(e.key){case"PageDown":t=this.pageDown();break;case"PageUp":t=this.pageUp()}return t||super[Te]&&super[Te](e)}get orientation(){return super.orientation||this[Be]&&this[Be].orientation||"both"}pageDown(){return super.pageDown&&super.pageDown(),Hs(this,!0)}pageUp(){return super.pageUp&&super.pageUp(),Hs(this,!1)}get[De](){return super[De]||Is(this)}}}function Bs(e,t,s){const n=e[Be].items,i=s?0:n.length-1,r=s?n.length:0,o=s?1:-1;let a,l,c=null;const{availableItemFlags:u}=e[Be];for(a=i;a!==r;a+=o)if((!u||u[a])&&(l=n[a].getBoundingClientRect(),l.top<=t&&t<=l.bottom)){c=n[a];break}if(!c||!l)return null;const d=getComputedStyle(c),h=d.paddingTop?parseFloat(d.paddingTop):0,p=d.paddingBottom?parseFloat(d.paddingBottom):0,m=l.top+h,g=m+c.clientHeight-h-p;return s&&m<=t||!s&&g>=t?a:a-o}function Hs(e,t){const s=e[Be].items,n=e[Be].currentIndex,i=e[De].getBoundingClientRect(),r=Bs(e,t?i.bottom:i.top,t);let o;if(r&&n===r){const i=s[n].getBoundingClientRect(),r=e[De].clientHeight;o=Bs(e,t?i.bottom+r:i.top-r,t)}else o=r;if(!o){const n=t?s.length-1:0;o=e[Q]?e[Q](e[Be],{direction:t?-1:1,index:n}):n}const a=o!==n;if(a){const t=e[Ie];e[Ie]=!0,e[Me]({currentIndex:o}),e[Ie]=t}return a}const Ws=Symbol("typedPrefix"),Ns=Symbol("prefixTimeout");function zs(e){return class extends e{constructor(){super(),Us(this)}[fe](e){if(super[fe]&&super[fe](e),null==e||0===e.length)return!1;const t=e.toLowerCase(),s=this[Be].texts.findIndex((s=>s.substr(0,e.length).toLowerCase()===t));if(s>=0){const e=this[Be].currentIndex;return this[Me]({currentIndex:s}),this[Be].currentIndex!==e}return!1}[Te](e){let t;switch(e.key){case"Backspace":!function(e){const t=e,s=t[Ws]?t[Ws].length:0;s>0&&(t[Ws]=t[Ws].substr(0,s-1)),e[fe](t[Ws]),Vs(e)}(this),t=!0;break;case"Escape":Us(this);break;default:e.ctrlKey||e.metaKey||e.altKey||1!==e.key.length||function(e,t){const s=e,n=s[Ws]||"";s[Ws]=n+t,e[fe](s[Ws]),Vs(e)}(this,e.key)}return t||super[Te]&&super[Te](e)}}}function Ys(e){const t=e;t[Ns]&&(clearTimeout(t[Ns]),t[Ns]=!1)}function Us(e){e[Ws]="",Ys(e)}function Vs(e){Ys(e),e[Ns]=setTimeout((()=>{Us(e)}),1e3)}function Gs(e){return class extends e{get[_](){const e=this[Fe]&&this[Fe].querySelector("slot:not([name])");return this[Fe]&&e||console.warn(`SlotContentMixin expects ${this.constructor.name} to define a shadow tree that includes a default (unnamed) slot.\nSee https://elix.org/documentation/SlotContentMixin.`),e}get[ee](){return Object.assign(super[ee]||{},{content:null})}[Oe](e){if(super[Oe]&&super[Oe](e),this[ie]){const e=this[_];e&&e.addEventListener("slotchange",(async()=>{this[Ie]=!0;const t=e.assignedNodes({flatten:!0});Object.freeze(t),this[Me]({content:t}),await Promise.resolve(),this[Ie]=!1}))}}}}function qs(e){return function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{items:null})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.content){const t=e.content,n=t?Array.prototype.filter.call(t,(e=>{return(t=e)instanceof Element&&(!t.localName||Ms.indexOf(t.localName)<0);var t})):null;n&&Object.freeze(n),Object.assign(s,{items:n})}return s}}}(Gs(e))}function Ks(e){return class extends e{constructor(){super(),this.addEventListener("mousedown",(e=>{0===e.button&&(this[Ie]=!0,this[Xe](e),this[Ie]=!1)}))}[Se](e){super[Se]&&super[Se](e),this[ie]&&Object.assign(this.style,{touchAction:"manipulation",mozUserSelect:"none",msUserSelect:"none",webkitUserSelect:"none",userSelect:"none"})}[Xe](e){const t=e.composedPath?e.composedPath()[0]:e.target,{items:s,currentItemRequired:n}=this[Be];if(s&&t instanceof Node){const i=E(s,t),r=i>=0?s[i]:null;(r&&!r.disabled||!r&&!n)&&(this[Me]({currentIndex:i}),e.stopPropagation())}}}}const $s=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"menuitem",role:e.role||"menu"})}get itemRole(){return this[Be].itemRole}set itemRole(e){this[Me]({itemRole:e})}[Se](e){super[Se]&&super[Se](e);const t=this[Be].items;if((e.items||e.itemRole)&&t){const{itemRole:e}=this[Be];t.forEach((t=>{e===Ps[t.localName]?t.removeAttribute("role"):t.setAttribute("role",e)}))}if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}(Es(ks(lt(Ls(Os(As(Rs(is(rs(js(zs(os(qs(Ks(G))))))))))))))),Xs=class extends $s{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,highlightCurrentItem:!0,orientation:"vertical",currentItemFocused:!1})}async flashCurrentItem(){const e=this[Be].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Me]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Me]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[Se](e){super[Se](e),this[ie]&&(this.addEventListener("disabledchange",(e=>{this[Ie]=!0;const t=e.target,{items:s}=this[Be],n=null===s?-1:s.indexOf(t);if(n>=0){const e=this[Be].availableItemFlags.slice();e[n]=!t.disabled,this[Me]({availableItemFlags:e})}this[Ie]=!1})),"PointerEvent"in window?this.addEventListener("pointerdown",(e=>this[Xe](e))):this.addEventListener("touchstart",(e=>this[Xe](e))),this.removeAttribute("tabindex"));const{currentIndex:t,items:s}=this[Be];if((e.items||e.currentIndex||e.highlightCurrentItem)&&s){const{highlightCurrentItem:e}=this[Be];s.forEach(((s,n)=>{s.toggleAttribute("current",e&&n===t)}))}(e.items||e.currentIndex||e.currentItemFocused||e.focusVisible)&&s&&s.forEach(((e,s)=>{const n=s===t,i=t<0&&0===s;this[Be].currentItemFocused?n||i||e.removeAttribute("tabindex"):(n||i)&&(e.tabIndex=0)}))}[Oe](e){if(super[Oe](e),!this[ie]&&e.currentIndex&&!this[Be].currentItemFocused){const{currentItem:e}=this[Be];(e instanceof HTMLElement?e:this).focus(),this[Me]({currentItemFocused:!0})}}get[De](){return this[we].content}[He](e,t){const s=super[He](e,t);if(t.currentIndex&&Object.assign(s,{currentItemFocused:!1}),t.items){const{items:t}=e,n=null===t?null:t.map((e=>!e.disabled));Object.assign(s,{availableItemFlags:n})}return s}get[Ze](){return D.html`
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
    `}},Zs=class extends Xs{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}};customElements.define("elix-menu",class extends Zs{});const Js=Symbol("documentMouseupListener");async function Qs(e){const t=this,s=t[Fe].elementsFromPoint(e.clientX,e.clientY);if(t.opened){const e=s.indexOf(t[we].source)>=0,n=t[we].popup,i=s.indexOf(n)>=0,r=n.frame&&s.indexOf(n.frame)>=0;e?t[Be].dragSelect&&(t[Ie]=!0,t[Me]({dragSelect:!1}),t[Ie]=!1):i||r||(t[Ie]=!0,await t.close(),t[Ie]=!1)}}function _s(e){e[Be].opened&&e.isConnected?e[Js]||(e[Js]=Qs.bind(e),document.addEventListener("mouseup",e[Js])):e[Js]&&(document.removeEventListener("mouseup",e[Js]),e[Js]=null)}function en(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{disabled:!1})}get disabled(){return this[Be].disabled}set disabled(e){this[Me]({disabled:e})}[Oe](e){if(super[Oe]&&super[Oe](e),e.disabled&&(this.toggleAttribute("disabled",this.disabled),this[Ie])){const e=new CustomEvent("disabled-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("disabledchange",{bubbles:!0});this.dispatchEvent(t)}}}}const tn=Symbol("closePromise"),sn=Symbol("closeResolve");function nn(e){return class extends e{attributeChangedCallback(e,t,s){if("opened"===e){const t=w(e,s);this.opened!==t&&(this.opened=t)}else super.attributeChangedCallback(e,t,s)}async close(e){super.close&&await super.close(),this[Me]({closeResult:e}),await this.toggle(!1)}get closed(){return this[Be]&&!this[Be].opened}get closeFinished(){return this[Be].openCloseEffects?"close"===this[Be].effect&&"after"===this[Be].effectPhase:this.closed}get closeResult(){return this[Be].closeResult}get[ee](){const e={closeResult:null,opened:!1};return this[je]&&Object.assign(e,{effect:"close",effectPhase:"after",openCloseEffects:!0}),Object.assign(super[ee]||{},e)}async open(){super.open&&await super.open(),this[Me]({closeResult:void 0}),await this.toggle(!0)}get opened(){return this[Be]&&this[Be].opened}set opened(e){this[Me]({closeResult:void 0}),this.toggle(e)}[Oe](e){if(super[Oe]&&super[Oe](e),e.opened&&this[Ie]){const e=new CustomEvent("opened-changed",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});this.dispatchEvent(e);const t=new CustomEvent("openedchange",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});if(this.dispatchEvent(t),this[Be].opened){const e=new CustomEvent("opened",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("open",{bubbles:!0});this.dispatchEvent(t)}else{const e=new CustomEvent("closed",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(e);const t=new CustomEvent("close",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(t)}}const t=this[sn];this.closeFinished&&t&&(this[sn]=null,this[tn]=null,t(this[Be].closeResult))}async toggle(e=!this.opened){if(super.toggle&&await super.toggle(e),e!==this[Be].opened){const t={opened:e};this[Be].openCloseEffects&&(t.effect=e?"open":"close","after"===this[Be].effectPhase&&(t.effectPhase="before")),await this[Me](t)}}whenClosed(){return this[tn]||(this[tn]=new Promise((e=>{this[sn]=e}))),this[tn]}}}function rn(e){return class extends e{get[ee](){return Object.assign(super[ee],{role:null})}[Se](e){if(super[Se]&&super[Se](e),e.role){const{role:e}=this[Be];e?this.setAttribute("role",e):this.removeAttribute("role")}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}const on=rn(G),an=class extends on{get[ee](){return Object.assign(super[ee],{role:"none"})}get[Ze](){return D.html`
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
    `}},ln=class extends G{get[Ze](){return D.html`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }
      </style>
      <slot></slot>
    `}},cn=Symbol("appendedToDocument"),un=Symbol("assignedZIndex"),dn=Symbol("restoreFocusToElement");function hn(e){const t=function(){const e=document.body.querySelectorAll("*"),t=Array.from(e,(e=>{const t=getComputedStyle(e);let s=0;if("static"!==t.position&&"auto"!==t.zIndex){const e=t.zIndex?parseInt(t.zIndex):0;s=isNaN(e)?0:e}return s}));return Math.max(...t)}()+1;e[un]=t,e.style.zIndex=t.toString()}function pn(e){const t=getComputedStyle(e).zIndex,s=e.style.zIndex,n=!isNaN(parseInt(s));if("auto"===t)return n;if("0"===t&&!n){const t=e.assignedSlot||(e instanceof ShadowRoot?e.host:e.parentNode);if(!(t instanceof HTMLElement))return!0;if(!pn(t))return!1}return!0}const mn=nn(function(e){return class extends e{get autoFocus(){return this[Be].autoFocus}set autoFocus(e){this[Me]({autoFocus:e})}get[ee](){return Object.assign(super[ee]||{},{autoFocus:!0,persistent:!1})}async open(){this[Be].persistent||this.isConnected||(this[cn]=!0,document.body.append(this)),super.open&&await super.open()}[Se](e){if(super[Se]&&super[Se](e),this[ie]&&this.addEventListener("blur",(e=>{const t=e.relatedTarget||document.activeElement;t instanceof HTMLElement&&(x(this,t)||(this.opened?this[dn]=t:(t.focus(),this[dn]=null)))})),(e.effectPhase||e.opened||e.persistent)&&!this[Be].persistent){const e=void 0===this.closeFinished?this.closed:this.closeFinished;this.style.display=e?"none":"",e?this[un]&&(this.style.zIndex="",this[un]=null):this[un]?this.style.zIndex=this[un]:pn(this)||hn(this)}}[Oe](e){if(super[Oe]&&super[Oe](e),this[ie]&&this[Be].persistent&&!pn(this)&&hn(this),e.opened&&this[Be].autoFocus)if(this[Be].opened){this[dn]||document.activeElement===document.body||(this[dn]=document.activeElement);const e=T(this);e&&e.focus()}else this[dn]&&(this[dn].focus(),this[dn]=null);!this[ie]&&!this[Be].persistent&&this.closeFinished&&this[cn]&&(this[cn]=!1,this.parentNode&&this.parentNode.removeChild(this))}}}(Gs(G)));function gn(e,t,s){if(!s||s.backdropPartType){const{backdropPartType:s}=t,n=e.getElementById("backdrop");n&&X(n,s)}if(!s||s.framePartType){const{framePartType:s}=t,n=e.getElementById("frame");n&&X(n,s)}}const fn=class extends mn{get backdrop(){return this[we]&&this[we].backdrop}get backdropPartType(){return this[Be].backdropPartType}set backdropPartType(e){this[Me]({backdropPartType:e})}get[ee](){return Object.assign(super[ee],{backdropPartType:an,framePartType:ln})}get frame(){return this[we].frame}get framePartType(){return this[Be].framePartType}set framePartType(e){this[Me]({framePartType:e})}[Se](e){super[Se](e),gn(this[Fe],this[Be],e)}[Oe](e){super[Oe](e),e.opened&&this[Be].content&&this[Be].content.forEach((e=>{e[J]&&e[J]()}))}get[Ze](){const e=D.html`
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
    `;return gn(e.content,this[Be]),e}},bn=Symbol("implicitCloseListener");async function yn(e){const t=this,s=e.relatedTarget||document.activeElement;s instanceof Element&&!x(t,s)&&(t[Ie]=!0,await t.close({canceled:"window blur"}),t[Ie]=!1)}async function wn(e){const t=this,s="resize"!==e.type||t[Be].closeOnWindowResize;!C(t,e)&&s&&(t[Ie]=!0,await t.close({canceled:"window "+e.type}),t[Ie]=!1)}const vn=rs(function(e){return class extends e{constructor(){super(),this.addEventListener("blur",yn.bind(this))}get closeOnWindowResize(){return this[Be].closeOnWindowResize}set closeOnWindowResize(e){this[Me]({closeOnWindowResize:e})}get[ee](){return Object.assign(super[ee]||{},{closeOnWindowResize:!0,role:"alert"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super.keydown&&super.keydown(e)||!1}[Se](e){if(super[Se]&&super[Se](e),e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}[Oe](e){var t;super[Oe]&&super[Oe](e),e.opened&&(this.opened?("requestIdleCallback"in window?window.requestIdleCallback:setTimeout)((()=>{var e;this.opened&&((e=this)[bn]=wn.bind(e),window.addEventListener("blur",e[bn]),window.addEventListener("resize",e[bn]),window.addEventListener("scroll",e[bn]))})):(t=this)[bn]&&(window.removeEventListener("blur",t[bn]),window.removeEventListener("resize",t[bn]),window.removeEventListener("scroll",t[bn]),t[bn]=null))}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}(fn));async function xn(e){const t=this;t[Ie]=!0,await t.close({canceled:"mousedown outside"}),t[Ie]=!1,e.preventDefault(),e.stopPropagation()}const Tn=class extends vn{[Se](e){super[Se](e),e.backdropPartType&&(this[we].backdrop.addEventListener("mousedown",xn.bind(this)),"PointerEvent"in window||this[we].backdrop.addEventListener("touchend",xn))}},Pn=Symbol("resizeListener"),En=en(ot(os(nn(G))));function Cn(e){const t=window.innerHeight,s=window.innerWidth,n=e[we].popup.getBoundingClientRect(),i=e.getBoundingClientRect(),r=n.height,o=n.width,{horizontalAlign:a,popupPosition:l,rightToLeft:c}=e[Be],u=i.top,d=Math.ceil(t-i.bottom),h=i.right,p=Math.ceil(s-i.left),m=r<=u,g=r<=d,f="below"===l,b=f&&(g||d>=u)||!f&&!m&&d>=u,y=b&&g||!b&&m?null:b?d:u,w=b?"below":"above";let v,x,T;if("stretch"===a)v=0,x=0,T=null;else{const e="left"===a||(c?"end"===a:"start"===a),t=e&&(o<=p||p>=h)||!e&&!(o<=h)&&p>=h;v=t?0:null,x=t?null:0,T=t&&p||!t&&h?null:t?p:h}e[Me]({calculatedFrameMaxHeight:y,calculatedFrameMaxWidth:T,calculatedPopupLeft:v,calculatedPopupPosition:w,calculatedPopupRight:x,popupMeasured:!0})}function In(e,t,s){if(!s||s.popupPartType){const{popupPartType:s}=t,n=e.getElementById("popup");n&&X(n,s)}if(!s||s.sourcePartType){const{sourcePartType:s}=t,n=e.getElementById("source");n&&X(n,s)}}const Sn=en(G),kn=class extends Sn{get[ee](){return Object.assign(super[ee],{direction:"down"})}get direction(){return this[Be].direction}set direction(e){this[Me]({direction:e})}[Se](e){if(super[Se](e),e.direction){const{direction:e}=this[Be];this[we].downIcon.style.display="down"===e?"block":"none",this[we].upIcon.style.display="up"===e?"block":"none"}}get[Ze](){return D.html`
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
    `}};function Ln(e,t,s){if(!s||s.popupTogglePartType){const{popupTogglePartType:s}=t,n=e.getElementById("popupToggle");n&&X(n,s)}}const On=lt(rs(function(e){return class extends e{connectedCallback(){super.connectedCallback(),_s(this)}get[ee](){return Object.assign(super[ee],{dragSelect:!0})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),_s(this)}[Oe](e){super[Oe](e),e.opened&&_s(this)}[He](e,t){const s=super[He](e,t);return t.opened&&e.opened&&Object.assign(s,{dragSelect:!0}),s}}}(function(e){return class extends e{get[ee](){return Object.assign(super[ee],{popupTogglePartType:kn})}get popupTogglePartType(){return this[Be].popupTogglePartType}set popupTogglePartType(e){this[Me]({popupTogglePartType:e})}[Se](e){if(super[Se](e),Ln(this[Fe],this[Be],e),e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[Be],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.disabled){const{disabled:e}=this[Be];this[we].popupToggle.disabled=e}}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="source"]');return t&&t.append(A`
        <div
          id="popupToggle"
          part="popup-toggle"
          exportparts="toggle-icon, down-icon, up-icon"
          tabindex="-1"
        >
          <slot name="toggle-icon"></slot>
        </div>
      `),Ln(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="popup-toggle"] {
          outline: none;
        }

        [part~="source"] {
          align-items: center;
          display: flex;
        }
      </style>
    `),e}}}(class extends En{get[ee](){return Object.assign(super[ee],{ariaHasPopup:"true",horizontalAlign:"start",popupHeight:null,popupMeasured:!1,popupPosition:"below",popupPartType:Tn,popupWidth:null,roomAbove:null,roomBelow:null,roomLeft:null,roomRight:null,sourcePartType:"div"})}get[ve](){return this[we].source}get frame(){return this[we].popup.frame}get horizontalAlign(){return this[Be].horizontalAlign}set horizontalAlign(e){this[Me]({horizontalAlign:e})}[Se](e){if(super[Se](e),In(this[Fe],this[Be],e),this[ie]||e.ariaHasPopup){const{ariaHasPopup:e}=this[Be];null===e?this[ve].removeAttribute("aria-haspopup"):this[ve].setAttribute("aria-haspopup",this[Be].ariaHasPopup)}if(e.popupPartType&&(this[we].popup.addEventListener("open",(()=>{this.opened||(this[Ie]=!0,this.open(),this[Ie]=!1)})),this[we].popup.addEventListener("close",(e=>{if(!this.closed){this[Ie]=!0;const t=e.detail.closeResult;this.close(t),this[Ie]=!1}}))),e.horizontalAlign||e.popupMeasured||e.rightToLeft){const{calculatedFrameMaxHeight:e,calculatedFrameMaxWidth:t,calculatedPopupLeft:s,calculatedPopupPosition:n,calculatedPopupRight:i,popupMeasured:r}=this[Be],o="below"===n,a=o?null:0,l=r?null:0,c=r?"absolute":"fixed",u=s,d=i,h=this[we].popup;Object.assign(h.style,{bottom:a,left:u,opacity:l,position:c,right:d});const p=h.frame;Object.assign(p.style,{maxHeight:e?e+"px":null,maxWidth:t?t+"px":null}),this[we].popupContainer.style.top=o?"":"0"}if(e.opened){const{opened:e}=this[Be];this[we].popup.opened=e}if(e.disabled&&"disabled"in this[we].source){const{disabled:e}=this[Be];this[we].source.disabled=e}}[Oe](e){var t;super[Oe](e),e.opened?this.opened?(t=this,setTimeout((()=>{t.opened&&(Cn(t),function(e){const t=e;t[Pn]=()=>{Cn(e)},window.addEventListener("resize",t[Pn])}(t))}))):function(e){const t=e;t[Pn]&&(window.removeEventListener("resize",t[Pn]),t[Pn]=null)}(this):this.opened&&!this[Be].popupMeasured&&Cn(this)}get popupPosition(){return this[Be].popupPosition}set popupPosition(e){this[Me]({popupPosition:e})}get popupPartType(){return this[Be].popupPartType}set popupPartType(e){this[Me]({popupPartType:e})}get sourcePartType(){return this[Be].sourcePartType}set sourcePartType(e){this[Me]({sourcePartType:e})}[He](e,t){const s=super[He](e,t);return t.opened&&!e.opened&&Object.assign(s,{calculatedFrameMaxHeight:null,calculatedFrameMaxWidth:null,calculatedPopupLeft:null,calculatedPopupPosition:null,calculatedPopupRight:null,popupMeasured:!1}),s}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),In(e.content,this[Be]),e}})))),An=class extends On{get[ee](){return Object.assign(super[ee],{sourcePartType:"button"})}[Te](e){let t;switch(e.key){case" ":case"ArrowDown":case"ArrowUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0)}if(t=super[Te]&&super[Te](e),!t&&this.opened&&!e.metaKey&&!e.altKey)switch(e.key){case"ArrowDown":case"ArrowLeft":case"ArrowRight":case"ArrowUp":case"End":case"Home":case"PageDown":case"PageUp":case" ":t=!0}return t}[Se](e){if(super[Se](e),this[ie]&&this[we].source.addEventListener("focus",(async e=>{const t=C(this[we].popup,e),s=null!==this[Be].popupHeight;!t&&this.opened&&s&&(this[Ie]=!0,await this.close(),this[Ie]=!1)})),e.opened){const{opened:e}=this[Be];this.toggleAttribute("opened",e),this[we].source.setAttribute("aria-expanded",e.toString())}e.sourcePartType&&this[we].source.addEventListener("mousedown",(e=>{if(this.disabled)return void e.preventDefault();const t=e;t.button&&0!==t.button||(setTimeout((()=>{this.opened||(this[Ie]=!0,this.open(),this[Ie]=!1)})),e.stopPropagation())})),e.popupPartType&&this[we].popup.removeAttribute("tabindex")}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},Dn=Symbol("documentMousemoveListener");function Mn(e){return class extends e{connectedCallback(){super.connectedCallback(),Rn(this)}get[ee](){return Object.assign(super[ee],{currentIndex:-1,hasHoveredOverItemSinceOpened:!1,popupList:null})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Rn(this)}[Te](e){let t=!1;switch(e.key){case"Enter":this.opened&&(jn(this),t=!0)}return t||super[Te]&&super[Te](e)||!1}[Se](e){if(super[Se]&&super[Se](e),e.popupList){const{popupList:e}=this[Be];e&&(e.addEventListener("mouseup",(async e=>{const t=this[Be].currentIndex;this[Be].dragSelect||t>=0?(e.stopPropagation(),this[Ie]=!0,await jn(this),this[Ie]=!1):e.stopPropagation()})),e.addEventListener("currentindexchange",(e=>{this[Ie]=!0;const t=e;this[Me]({currentIndex:t.detail.currentIndex}),this[Ie]=!1})))}if(e.currentIndex||e.popupList){const{currentIndex:e,popupList:t}=this[Be];t&&"currentIndex"in t&&(t.currentIndex=e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.opened){if(this[Be].opened){const{popupList:e}=this[Be];e.scrollCurrentItemIntoView&&setTimeout((()=>{e.scrollCurrentItemIntoView()}))}Rn(this)}}[He](e,t){const s=super[He]?super[He](e,t):{};return t.opened&&e.opened&&Object.assign(s,{hasHoveredOverItemSinceOpened:!1}),s}}}function Fn(e){const t=this,{hasHoveredOverItemSinceOpened:s,opened:n}=t[Be];if(n){const n=e.composedPath?e.composedPath()[0]:e.target;if(n&&n instanceof Node){const e=t.items,i=E(e,n),r=e[i],o=r&&!r.disabled?i:-1;(s||o>=0)&&o!==t[Be].currentIndex&&(t[Ie]=!0,t[Me]({currentIndex:o}),o>=0&&!s&&t[Me]({hasHoveredOverItemSinceOpened:!0}),t[Ie]=!1)}}}function Rn(e){e[Be].opened&&e.isConnected?e[Dn]||(e[Dn]=Fn.bind(e),document.addEventListener("mousemove",e[Dn])):e[Dn]&&(document.removeEventListener("mousemove",e[Dn]),e[Dn]=null)}async function jn(e){const t=e[Ie],s=e[Be].currentIndex>=0,n=s?e.items[e[Be].currentIndex]:void 0,i=e[Be].popupList;s&&"flashCurrentItem"in i&&await i.flashCurrentItem();const r=e[Ie];e[Ie]=t,await e.close(n),e[Ie]=r}const Bn=Mn(An);function Hn(e,t,s){if(!s||s.menuPartType){const{menuPartType:s}=t,n=e.getElementById("menu");n&&X(n,s)}}const Wn=class extends Bn{get[ee](){return Object.assign(super[ee],{menuPartType:Xs})}get items(){const e=this[we]&&this[we].menu;return e?e.items:null}get menuPartType(){return this[Be].menuPartType}set menuPartType(e){this[Me]({menuPartType:e})}[Se](e){super[Se](e),Hn(this[Fe],this[Be],e),e.menuPartType&&(this[we].menu.addEventListener("blur",(async e=>{const t=e.relatedTarget||document.activeElement;this.opened&&!x(this[we].menu,t)&&(this[Ie]=!0,await this.close(),this[Ie]=!1)})),this[we].menu.addEventListener("mousedown",(e=>{0===e.button&&this.opened&&(e.stopPropagation(),e.preventDefault())})))}[Oe](e){super[Oe](e),e.menuPartType&&this[Me]({popupList:this[we].menu})}[He](e,t){const s=super[He](e,t);return t.opened&&!e.opened&&Object.assign(s,{currentIndex:-1}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <div id="menu" part="menu">
          <slot></slot>
        </div>
      `),Hn(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="menu"] {
          max-height: 100%;
        }
      </style>
    `),e}},Nn=class extends ms{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          [part~="inner"] {
            background: #eee;
            border: 1px solid #ccc;
            padding: 0.25em 0.5em;
          }
        </style>
      `),e}},zn=class extends kn{get[Ze](){const e=super[Ze],t=e.content.getElementById("downIcon"),s=A`
      <svg
        id="downIcon"
        part="toggle-icon down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 0 l5 5 5 -5 z" />
      </svg>
    `.firstElementChild;t&&s&&$(t,s);const n=e.content.getElementById("upIcon"),i=A`
      <svg
        id="upIcon"
        part="toggle-icon up-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 5 l5 -5 5 5 z" />
      </svg>
    `.firstElementChild;return n&&i&&$(n,i),e.content.append(A`
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
      `),e}},Yn=class extends an{},Un=class extends ln{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 0px 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
          }
        </style>
      `),e}},Vn=class extends Tn{get[ee](){return Object.assign(super[ee],{backdropPartType:Yn,framePartType:Un})}},Gn=class extends Wn{get[ee](){return Object.assign(super[ee],{menuPartType:Zs,popupPartType:Vn,popupTogglePartType:zn,sourcePartType:Nn})}get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          [part~="menu"] {
            background: window;
            border: none;
            padding: 0.5em 0;
          }
        </style>
      `),e}};function qn(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if("current"===e){const t=w(e,s);this.current!==t&&(this.current=t)}else super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{current:!1})}[Se](e){if(super[Se](e),e.current){const{current:e}=this[Be];S(this,"current",e)}}[Oe](e){if(super[Oe]&&super[Oe](e),e.current){const{current:e}=this[Be],t=new CustomEvent("current-changed",{bubbles:!0,detail:{current:e}});this.dispatchEvent(t);const s=new CustomEvent("currentchange",{bubbles:!0,detail:{current:e}});this.dispatchEvent(s)}}get current(){return this[Be].current}set current(e){this[Me]({current:e})}}}customElements.define("elix-menu-button",class extends Gn{});class Kn extends(qn(en(Vt(G)))){}const $n=Kn,Xn=class extends $n{get[Ze](){return D.html`
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
    `}};customElements.define("elix-menu-item",class extends Xn{});const Zn=class extends G{get disabled(){return!0}[Se](e){super[Se](e),this[ie]&&this.setAttribute("aria-hidden","true")}},Jn=class extends Zn{get[Ze](){return D.html`
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
    `}};customElements.define("elix-menu-separator",class extends Jn{});const Qn=class extends An{get[ee](){return Object.assign(super[ee],{popupPartType:Vn,sourcePartType:Nn})}};customElements.define("elix-popup-button",class extends Qn{}),customElements.define("elix-popup",class extends Vn{});const _n=Symbol("previousBodyStyleOverflow"),ei=Symbol("previousDocumentMarginRight");function ti(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:"dialog"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super[Te]&&super[Te](e)||!1}[Se](e){if(super[Se]&&super[Se](e),e.opened)if(this[Be].opened&&document.documentElement){const e=document.documentElement.clientWidth,t=window.innerWidth-e;this[_n]=document.body.style.overflow,this[ei]=t>0?document.documentElement.style.marginRight:null,document.body.style.overflow="hidden",t>0&&(document.documentElement.style.marginRight=t+"px")}else null!=this[_n]&&(document.body.style.overflow=this[_n],this[_n]=null),null!=this[ei]&&(document.documentElement.style.marginRight=this[ei],this[ei]=null);if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[Ae]||this[Me]({role:e})}}}const si=Symbol("wrap"),ni=Symbol("wrappingFocus");function ii(e){return class extends e{[Te](e){const t=T(this[Fe]);if(t){const s=document.activeElement&&(document.activeElement===t||document.activeElement.contains(t)),n=this[Fe].activeElement,i=n&&(n===t||x(n,t));(s||i)&&"Tab"===e.key&&e.shiftKey&&(this[ni]=!0,this[we].focusCatcher.focus(),this[ni]=!1)}return super[Te]&&super[Te](e)||!1}[Se](e){super[Se]&&super[Se](e),this[ie]&&this[we].focusCatcher.addEventListener("focus",(()=>{if(!this[ni]){const e=T(this[Fe]);e&&e.focus()}}))}[si](e){const t=A`
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
      `,s=t.getElementById("focusCaptureContainer");s&&(e.replaceWith(t),s.append(e))}}}ii.wrap=si;const ri=ii,oi=class extends an{constructor(){super(),"PointerEvent"in window||this.addEventListener("touchmove",(e=>{1===e.touches.length&&e.preventDefault()}))}},ai=ti(ri(rs(fn))),li=class extends ai{get[ee](){return Object.assign(super[ee],{backdropPartType:oi,tabIndex:-1})}get[Ze](){const e=super[Ze],t=e.content.querySelector("#frame");return this[ri.wrap](t),e.content.append(A`
        <style>
          :host {
            height: 100%;
            left: 0;
            pointer-events: initial;
            top: 0;
            width: 100%;
          }
        </style>
      `),e}},ci=class extends oi{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            background: rgba(0, 0, 0, 0.2);
          }
        </style>
      `),e}};function ui(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{backdropPartType:ci,framePartType:Un})}}}class di extends(ui(li)){}const hi=di;function pi(e){return class extends e{get[ee](){return Object.assign(super[ee],{selectedText:""})}[oe](e){return super[oe]?super[oe](e):Fs(e)}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?this[oe](i):"";Object.assign(s,{selectedText:r})}return s}get selectedText(){return this[Be].selectedText}set selectedText(e){const{items:t}=this[Be],s=t?function(e,t,s){return e.findIndex((e=>t(e)===s))}(t,this[oe],e):-1;this[Me]({selectedIndex:s})}}}function mi(e){return class extends e{get[ee](){return Object.assign(super[ee],{value:null})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?i.getAttribute("value"):"";Object.assign(s,{value:r})}return s}get value(){return this[Be].value}set value(e){const{items:t}=this[Be],s=t?function(e,t){return e.findIndex((e=>e.getAttribute("value")===t))}(t,e):-1;this[Me]({selectedIndex:s})}}}function gi(e){return class extends e{attributeChangedCallback(e,t,s){"selected-index"===e?this.selectedIndex=Number(s):super.attributeChangedCallback(e,t,s)}[Oe](e){if(super[Oe]&&super[Oe](e),e.selectedIndex&&this[Ie]){const e=this[Be].selectedIndex,t=new CustomEvent("selected-index-changed",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedindexchange",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(s)}}get selectedIndex(){const{items:e,selectedIndex:t}=this[Be];return e&&e.length>0?t:-1}set selectedIndex(e){isNaN(e)||this[Me]({selectedIndex:e})}get selectedItem(){const{items:e,selectedIndex:t}=this[Be];return e&&e[t]}set selectedItem(e){const{items:t}=this[Be];if(!t)return;const s=t.indexOf(e);s>=0&&this[Me]({selectedIndex:s})}}}customElements.define("elix-dialog",class extends hi{});const fi=Es(tt(ns(Os(As(Mn(pi(mi(gi(qs(An))))))))));function bi(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&X(n,s)}if(!s||s.valuePartType){const{valuePartType:s}=t,n=e.getElementById("value");n&&X(n,s)}}const yi=class extends fi{[Z](e,t){L(t,(e?[...e.childNodes]:[]).map((e=>e.cloneNode(!0))))}get[ee](){return Object.assign(super[ee],{accessibleOptions:null,ariaHasPopup:"listbox",listPartType:"div",selectedIndex:-1,selectedItem:null,valuePartType:"div"})}get items(){const e=this[we]&&this[we].list;return e?e.items:null}get listPartType(){return this[Be].listPartType}set listPartType(e){this[Me]({listPartType:e})}[Se](e){if(super[Se](e),bi(this[Fe],this[Be],e),e.items||e.selectedIndex){const{items:e,selectedIndex:t}=this[Be],s=e?e[t]:null;this[Z](s,this[we].value),e&&e.forEach((e=>{"selected"in e&&(e.selected=e===s)}))}if(e.sourcePartType){const e=this[we].source;e.inner&&e.inner.setAttribute("role","none")}}[Oe](e){super[Oe](e),e.listPartType&&this[Me]({popupList:this[we].list})}[He](e,t){const s=super[He](e,t);if(t.items){const t=(e.items||[]).map((e=>{const t=document.createElement("option");return t.textContent=e.textContent,t}));Object.assign(s,{accessibleOptions:t})}if(t.opened&&e.opened&&Object.assign(s,{currentIndex:e.selectedIndex}),t.opened){const{closeResult:n,currentIndex:i,opened:r}=e,o=t.opened&&!r,a=n&&n.canceled;o&&!a&&i>=0&&Object.assign(s,{selectedIndex:i})}if(t.items||t.selectedIndex){const{items:t,opened:n,selectedIndex:i}=e;!n&&i<0&&t&&t.length>0&&Object.assign(s,{selectedIndex:0})}return s}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="source"]');t&&$(t,A` <div id="value" part="value"></div> `);const s=e.content.querySelector("slot:not([name])");s&&s.replaceWith(A`
        <div id="list" part="list">
          <slot></slot>
        </div>
      `);const n=e.content.querySelector('[part~="source"]');return n&&(n.setAttribute("aria-activedescendant","value"),n.setAttribute("aria-autocomplete","none"),n.setAttribute("aria-controls","list"),n.setAttribute("role","combobox")),bi(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="list"] {
          max-height: 100%;
        }
      </style>
    `),e}get valuePartType(){return this[Be].valuePartType}set valuePartType(e){this[Me]({valuePartType:e})}},wi=rn(et(Es(ks(Ls(ot(ns(Os(As(Rs(is(rs(js(zs(os(gi(pi(mi(qs(Ks(G)))))))))))))))))))),vi=class extends wi{get[ee](){return Object.assign(super[ee],{highlightCurrentItem:!0,orientation:"vertical",role:"listbox"})}async flashCurrentItem(){const e=this[Be].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Me]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Me]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[Se](e){if(super[Se](e),e.items||e.currentIndex||e.highlightCurrentItem){const{currentIndex:e,items:t,highlightCurrentItem:s}=this[Be];t&&t.forEach(((t,n)=>{const i=n===e;t.toggleAttribute("current",s&&i),t.setAttribute("aria-selected",String(i))}))}}get[De](){return this[we].container}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),e}},xi=class extends vi{get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),e}},Ti=class extends yi{get[ee](){return Object.assign(super[ee],{listPartType:xi,popupPartType:Vn,sourcePartType:Nn,popupTogglePartType:zn})}};customElements.define("elix-dropdown-list",class extends Ti{});class Pi extends(rn(qn(en(Vt(G))))){get[ee](){return Object.assign(super[ee],{role:"option"})}get[Ze](){return D.html`
      <style>
        :host {
          display: block;
        }
      </style>
      <slot></slot>
    `}}const Ei=Pi,Ci=class extends Ei{get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
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

    `),e}};customElements.define("elix-option",class extends Ci{});const Ii=Symbol("deferToScrolling"),Si=Symbol("multiTouch"),ki=Symbol("previousTime"),Li=Symbol("previousVelocity"),Oi=Symbol("previousX"),Ai=Symbol("previousY"),Di=Symbol("startX"),Mi=Symbol("startY"),Fi=Symbol("touchSequenceAxis");function Ri(e){return"pen"===e.pointerType||"touch"===e.pointerType&&e.isPrimary}function ji(e,t,s,n){const i=e,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[Be],l=t-i[Oi],c=s-i[Ai],u=Date.now(),d="vertical"===r?c:l,h=d/(u-i[ki])*1e3;i[Oi]=t,i[Ai]=s,i[ki]=u,i[Li]=h;const p=Math.abs(c)>Math.abs(l)?"vertical":"horizontal";if(null===i[Fi])i[Fi]=p;else if(p!==i[Fi])return!0;if(p!==r)return!1;if(i[Ii]&&Cs(n,r,d<0))return!1;i[Di]||(i[Di]=t),i[Mi]||(i[Mi]=s);const m=function(e,t,s){const{swipeAxis:n}=e[Be],i=e,r="vertical"===n,o=r?s-i[Mi]:t-i[Di],a=r?e[$e].offsetHeight:e[$e].offsetWidth;return a>0?o/a:0}(e,t,s),g=Math.max(Math.min(m,o),a);return e[Be].swipeFraction!==g&&(i[Ii]=!1,e[Me]({swipeFraction:g}),!0)}function Bi(e,t,s,n){const i=e[Li],{swipeAxis:r,swipeFraction:o}=e[Be],a="vertical"===r;let l=!1;if(e[Ii]&&(l=Cs(n,r,i<0)),!l){let t;if(i>=800&&o>=0?(t=!0,a?e[Me]({swipeDownWillCommit:!0}):e[Me]({swipeRightWillCommit:!0})):i<=-800&&o<=0?(t=!1,a?e[Me]({swipeUpWillCommit:!0}):e[Me]({swipeLeftWillCommit:!0})):e[Be].swipeLeftWillCommit||e[Be].swipeUpWillCommit?t=!1:(e[Be].swipeRightWillCommit||e[Be].swipeDownWillCommit)&&(t=!0),void 0!==t){const s=a?t?We:Ge:t?Ue:ze;s&&e[s]&&e[s]()}}e[Fi]=null,e[Me]({swipeFraction:null})}function Hi(e,t,s){const n=e;n[Ii]=!0,n[ki]=Date.now(),n[Li]=0,n[Oi]=t,n[Ai]=s,n[Di]=null,n[Mi]=null,n[Fi]=null,e[Me]({swipeFraction:0}),e[Ke]&&e[Ke](t,s)}const Wi=Symbol("absorbDeceleration"),Ni=Symbol("deferToScrolling"),zi=Symbol("lastDeltaX"),Yi=Symbol("lastDeltaY"),Ui=Symbol("lastWheelTimeout"),Vi=Symbol("postGestureDelayComplete"),Gi=Symbol("wheelDistance"),qi=Symbol("wheelSequenceAxis");function Ki(e){const t=e;t[Wi]=!1,t[Ni]=!0,t[zi]=0,t[Yi]=0,t[Vi]=!0,t[Gi]=0,t[qi]=null,t[Ui]&&(clearTimeout(t[Ui]),t[Ui]=null)}const $i=ti(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{enableEffects:!1})}[Oe](e){super[Oe]&&super[Oe](e),this[ie]&&setTimeout((()=>{this[Me]({enableEffects:!0})}))}}}(ri(rs(os(function(e){return class extends e{[Se](e){super[Se]&&super[Se](e),this[ie]&&("TouchEvent"in window?(this.addEventListener("touchstart",(async e=>{if(this[Ie]=!0,!this[Si]){if(1===e.touches.length){const{clientX:t,clientY:s}=e.changedTouches[0];Hi(this,t,s)}else this[Si]=!0;await Promise.resolve(),this[Ie]=!1}})),this.addEventListener("touchmove",(async e=>{if(this[Ie]=!0,!this[Si]&&1===e.touches.length&&e.target){const{clientX:t,clientY:s}=e.changedTouches[0];ji(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Ie]=!1})),this.addEventListener("touchend",(async e=>{if(this[Ie]=!0,0===e.touches.length&&e.target){if(!this[Si]){const{clientX:t,clientY:s}=e.changedTouches[0];Bi(this,0,0,e.target)}this[Si]=!1}await Promise.resolve(),this[Ie]=!1}))):"PointerEvent"in window&&(this.addEventListener("pointerdown",(async e=>{if(this[Ie]=!0,Ri(e)){const{clientX:t,clientY:s}=e;Hi(this,t,s)}await Promise.resolve(),this[Ie]=!1})),this.addEventListener("pointermove",(async e=>{if(this[Ie]=!0,Ri(e)&&e.target){const{clientX:t,clientY:s}=e;ji(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Ie]=!1})),this.addEventListener("pointerup",(async e=>{if(this[Ie]=!0,Ri(e)&&e.target){const{clientX:t,clientY:s}=e;Bi(this,0,0,e.target)}await Promise.resolve(),this[Ie]=!1}))),this.style.touchAction="TouchEvent"in window?"manipulation":"none")}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeStartX:null,swipeStartY:null,swipeUpWillCommit:!1})}get[$e](){return super[$e]||this}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{constructor(){super(),this.addEventListener("wheel",(async e=>{this[Ie]=!0,function(e,t){const s=e;s[Ui]&&clearTimeout(s[Ui]),s[Ui]=setTimeout((async()=>{e[Ie]=!0,async function(e){let t;e[Be].swipeDownWillCommit?t=We:e[Be].swipeLeftWillCommit?t=ze:e[Be].swipeRightWillCommit?t=Ue:e[Be].swipeUpWillCommit&&(t=Ge),Ki(e),e[Me]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1}),t&&e[t]&&await e[t]()}(e),await Promise.resolve(),s[Ie]=!1}),100);const n=t.deltaX,i=t.deltaY,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[Be],l="vertical"===r,c=l?Math.sign(i)*(i-s[Yi]):Math.sign(n)*(n-s[zi]);s[zi]=n,s[Yi]=i;const u=null===s[qi],d=Math.abs(i)>Math.abs(n)?"vertical":"horizontal";if(!u&&d!==s[qi])return!0;if(d!==r)return!1;if(!s[Vi])return!0;if(c>0)s[Wi]=!1;else if(s[Wi])return!0;if(s[Ni]&&Cs(e[De]||e,r,(l?i:n)>0))return!1;s[Ni]=!1,u&&(s[qi]=d,e[Ke]&&e[Ke](t.clientX,t.clientY)),s[Gi]-=l?i:n;const h=l?s[$e].offsetHeight:s[$e].offsetWidth;let p=h>0?s[Gi]/h:0;p=Math.sign(p)*Math.min(Math.abs(p),1);const m=Math.max(Math.min(p,o),a);let g;return-1===m?g=l?Ge:ze:1===m&&(g=l?We:Ue),g?function(e,t){e[t]&&e[t]();const s=e;s[Wi]=!0,s[Ni]=!0,s[Vi]=!1,s[Gi]=0,s[qi]=null,setTimeout((()=>{s[Vi]=!0}),250),e[Me]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}(e,g):e[Me]({swipeFraction:m}),!0}(this,e)&&(e.preventDefault(),e.stopPropagation()),await Promise.resolve(),this[Ie]=!1})),Ki(this)}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}get[$e](){return super[$e]||this}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{get[ne](){return super[ne]||this}[Se](e){super[Se]&&super[Se](e),this[ie]&&(this[ne]===this?this:this[Fe]).addEventListener("transitionend",(e=>{const t=this[Be].effectEndTarget||this[ne];e.target===t&&this[Me]({effectPhase:"after"})}))}[Oe](e){if(super[Oe]&&super[Oe](e),e.effect||e.effectPhase){const{effect:e,effectPhase:t}=this[Be],s=new CustomEvent("effect-phase-changed",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(s);const n=new CustomEvent("effectphasechange",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(n),e&&("after"!==t&&this.offsetHeight,"before"===t&&this[Me]({effectPhase:"during"}))}}async[je](e){await this[Me]({effect:e,effectPhase:"before"})}}}(fn))))))));async function Xi(e){e[Me]({effect:"close",effectPhase:"during"}),await e.close()}async function Zi(e){e[Me]({effect:"open",effectPhase:"during"}),await e.open()}const Ji=class extends $i{get[ee](){return Object.assign(super[ee],{backdropPartType:oi,drawerTransitionDuration:250,fromEdge:"start",gripSize:null,openedFraction:0,openedRenderedFraction:0,persistent:!0,role:"landmark",showTransition:!1,tabIndex:-1})}get[ne](){return this[we].frame}get fromEdge(){return this[Be].fromEdge}set fromEdge(e){this[Me]({fromEdge:e})}get gripSize(){return this[Be].gripSize}set gripSize(e){this[Me]({gripSize:e})}[Se](e){if(super[Se](e),e.backdropPartType&&this[we].backdrop.addEventListener("click",(async()=>{this[Ie]=!0,await this.close(),this[Ie]=!1})),e.gripSize||e.opened||e.swipeFraction){const{gripSize:e,opened:t,swipeFraction:s}=this[Be],n=null!==s,i=t||n;this.style.pointerEvents=i?"initial":"none";const r=!(null!==e||i);this[we].frame.style.clipPath=r?"inset(0px)":""}if(e.effect||e.effectPhase||e.fromEdge||e.gripSize||e.openedFraction||e.rightToLeft||e.swipeFraction){const{drawerTransitionDuration:e,effect:t,effectPhase:s,fromEdge:n,gripSize:i,openedFraction:r,openedRenderedFraction:o,rightToLeft:a,showTransition:l,swipeFraction:c}=this[Be],u="left"===n||"top"===n||"start"===n&&!a||"end"===n&&a?-1:1,d=u*(1-r);null!==c||"open"===t&&"before"===s?this[we].backdrop.style.visibility="visible":"close"===t&&"after"===s&&(this[we].backdrop.style.visibility="hidden");const h=Math.abs(r-o),p=l?h*(e/1e3):0,m=100*d+"%",g=i?i*-u*(1-r):0,f=`translate${"top"===n||"bottom"===n?"Y":"X"}(${0===g?m:`calc(${m} + ${g}px)`})`;Object.assign(this[we].frame.style,{transform:f,transition:l?`transform ${p}s`:""})}if(e.fromEdge||e.rightToLeft){const{fromEdge:e,rightToLeft:t}=this[Be],s={bottom:0,left:0,right:0,top:0},n={bottom:"top",left:"right",right:"left",top:"bottom"};n.start=n[t?"right":"left"],n.end=n[t?"left":"right"],Object.assign(this.style,s,{[n[e]]:null});const i={bottom:"flex-end",end:"flex-end",left:t?"flex-end":"flex-start",right:t?"flex-start":"flex-end",start:"flex-start",top:"flex-start"};this.style.flexDirection="top"===e||"bottom"===e?"column":"row",this.style.justifyContent=i[e]}e.opened&&this.setAttribute("aria-expanded",this[Be].opened.toString())}[Oe](e){super[Oe](e),e.opened&&S(this,"opened",this[Be].opened),e.openedFraction&&this[Me]({openedRenderedFraction:this[Be].openedFraction})}[He](e,t){const s=super[He]?super[He](e,t):{};if(t.fromEdge){const{fromEdge:t}=e,n="top"===t||"bottom"===t?"vertical":"horizontal";Object.assign(s,{swipeAxis:n})}if(t.effect||t.effectPhase||t.fromEdge||t.rightToLeft||t.swipeFraction){const{effect:t,effectPhase:n,fromEdge:i,rightToLeft:r,swipeFraction:o}=e,a="open"===t&&"before"!==n||"close"===t&&"before"===n,l="left"===i||"top"===i||"start"===i&&!r||"end"===i&&r,c=.999,u=l&&!a||!l&&a,d=u?0:-c,h=u?c:0,p=l?-1:1;let m=a?1:0;null!==o&&(m-=p*Math.max(Math.min(o,h),d)),Object.assign(s,{openedFraction:m})}if(t.enableEffects||t.effect||t.effectPhase||t.swipeFraction){const{enableEffects:t,effect:n,effectPhase:i,swipeFraction:r}=e,o=null!==r,a=t&&!o&&n&&("during"===i||"after"===i);Object.assign(s,{showTransition:a})}return s}async[We](){const{fromEdge:e}=this[Be];"top"===e?Zi(this):"bottom"===e&&Xi(this)}async[ze](){const{fromEdge:e,rightToLeft:t}=this[Be],s="left"===e||"start"===e&&!t||"end"===e&&t;"right"===e||"start"===e&&t||"end"===e&&!t?Zi(this):s&&Xi(this)}async[Ue](){const{fromEdge:e,rightToLeft:t}=this[Be],s="right"===e||"start"===e&&t||"end"===e&&!t;"left"===e||"start"===e&&!t||"end"===e&&t?Zi(this):s&&Xi(this)}async[Ge](){const{fromEdge:e}=this[Be];"bottom"===e?Zi(this):"top"===e&&Xi(this)}get[De](){return this[we].frame}get[$e](){return this[we].frame}get[Ze](){const e=super[Ze],t=e.content.querySelector("#frameContent");return this[ri.wrap](t),e.content.append(A`
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
      `),e}};class Qi extends(function(e){return class extends e{[Se](e){if(super[Se]&&super[Se](e),e.openedFraction){const{drawerTransitionDuration:e,openedFraction:t,openedRenderedFraction:s,showTransition:n}=this[Be],i=Math.abs(t-s),r=n?i*(e/1e3):0;Object.assign(this[we].backdrop.style,{opacity:t,transition:n?`opacity ${r}s linear`:""})}}}}(ui(Ji))){}const _i=Qi;customElements.define("elix-drawer",class extends _i{})})();