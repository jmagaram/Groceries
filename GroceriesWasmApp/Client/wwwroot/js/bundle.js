(()=>{"use strict";const e=Symbol("defaultState"),t=Symbol("delegatesFocus"),s=Symbol("firstRender"),n=Symbol("focusTarget"),i=Symbol("hasDynamicTemplate"),r=Symbol("ids"),o=Symbol("nativeInternals"),a=Symbol("raiseChangeEvents"),l=Symbol("render"),c=Symbol("renderChanges"),u=Symbol("rendered"),d=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),m=Symbol("shadowRootMode"),g=Symbol("state"),f=Symbol("stateEffects"),b=Symbol("template"),y=Symbol("mousedownListener");function w(e,t){return"boolean"==typeof t?t:"string"==typeof t&&(""===t||e.toLowerCase()===t.toLowerCase())}function v(e){for(const t of C(e)){const e=t[n]||t,s=e;if(e instanceof HTMLElement&&e.tabIndex>=0&&!s.disabled&&!(e instanceof HTMLSlotElement))return e}return null}function x(e,t){let s=t;for(;s;){const t=s.assignedSlot||s.parentNode||s.host;if(t===e)return!0;s=t}return!1}function T(e){const t=O(e,(e=>e instanceof HTMLElement&&e.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&e.tabIndex>=0)),{value:s}=t.next();return s instanceof HTMLElement?s:null}function E(e,t){e[y]&&e.removeEventListener("mousedown",e[y]),t&&(e[y]=e=>{if(0!==e.button)return;const s=v(t[n]||t);s&&(s.focus(),e.preventDefault())},e.addEventListener("mousedown",e[y]))}function P(e,t){return Array.prototype.findIndex.call(e,(e=>e===t||x(e,t)))}function I(e,t){const s=t.composedPath()[0];return e===s||x(e,s)}function*C(e){e&&(yield e,yield*function*(e){let t=e;for(;t=t instanceof HTMLElement&&t.assignedSlot?t.assignedSlot:t instanceof ShadowRoot?t.host:t.parentNode,t;)yield t}(e))}function S(e,t,s){e.toggleAttribute(t,s),e[o]&&e[o].states&&e[o].states.toggle(t,s)}const k={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function L(e,t){const s=[...t],n=e.childNodes.length,i=s.length,r=Math.max(n,i);for(let t=0;t<r;t++){const r=e.childNodes[t],o=s[t];t>=n?e.append(o):t>=i?e.removeChild(e.childNodes[i]):r!==o&&(s.indexOf(r,t)>=t?e.insertBefore(o,r):e.replaceChild(o,r))}}function*O(e,t){let s;if(t(e)&&(yield e),e instanceof HTMLElement&&e.shadowRoot)s=e.shadowRoot.children;else{const t=e instanceof HTMLSlotElement?e.assignedNodes({flatten:!0}):[];s=t.length>0?t:e.childNodes}if(s)for(let e=0;e<s.length;e++)yield*O(s[e],t)}const A=(e,...t)=>D.html(e,...t).content,D={html(e,...t){const s=document.createElement("template");return s.innerHTML=String.raw(e,...t),s}},M={tabindex:"tabIndex"},F={tabIndex:"tabindex"};function j(e){if(e===HTMLElement)return[];const t=Object.getPrototypeOf(e.prototype).constructor;let s=t.observedAttributes;s||(s=j(t));const n=Object.getOwnPropertyNames(e.prototype).filter((t=>{const s=Object.getOwnPropertyDescriptor(e.prototype,t);return s&&"function"==typeof s.set})).map((e=>function(e){let t=F[e];if(!t){const s=/([A-Z])/g;t=e.replace(s,"-$1").toLowerCase(),F[e]=t}return t}(e))).filter((e=>s.indexOf(e)<0));return s.concat(n)}const R=Symbol("state"),H=Symbol("raiseChangeEventsInNextRender"),B=Symbol("changedSinceLastRender");function N(e,t){const s={};for(const r in t)n=t[r],i=e[r],(n instanceof Date&&i instanceof Date?n.getTime()===i.getTime():n===i)||(s[r]=!0);var n,i;return s}const W=new Map,z=Symbol("shadowIdProxy"),Y=Symbol("proxyElement"),U={get(e,t){const s=e[Y][p];return s&&"string"==typeof t?s.getElementById(t):null}};function q(e){let t=e[i]?void 0:W.get(e.constructor);if(void 0===t){if(t=e[b],t&&!(t instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${e.constructor.name} must return an HTMLTemplateElement.`;e[i]||W.set(e.constructor,t||null)}return t}const V=function(e){return class extends e{attributeChangedCallback(e,t,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(e,t,s),s!==t&&!this[d]){const t=function(e){let t=M[e];if(!t){const s=/-([a-z])/g;t=e.replace(s,(e=>e[1].toUpperCase())),M[e]=t}return t}(e);if(t in this){const n=k[e]?w(e,s):s;this[t]=n}}}static get observedAttributes(){return j(this)}}}(function(t){class n extends t{constructor(){super(),this[s]=void 0,this[a]=!1,this[B]=null,this[h](this[e])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[c]()}get[e](){return super[e]||{}}[l](e){super[l]&&super[l](e)}[c](){void 0===this[s]&&(this[s]=!0);const e=this[B];if(this[s]||e){const t=this[a];this[a]=this[H],this[d]=!0,this[l](e),this[d]=!1,this[B]=null,this[u](e),this[s]=!1,this[a]=t,this[H]=t}}[u](e){super[u]&&super[u](e)}async[h](e){this[d]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:t,changed:n}=function(e,t){const s=Object.assign({},e[R]),n={};let i=t;for(;;){const t=N(s,i);if(0===Object.keys(t).length)break;Object.assign(s,i),Object.assign(n,t),i=e[f](s,t)}return{state:s,changed:n}}(this,e);if(this[R]&&0===Object.keys(n).length)return;Object.freeze(t),this[R]=t,this[a]&&(this[H]=!0);const i=void 0===this[s]||null!==this[B];this[B]=Object.assign(this[B]||{},n),this.isConnected&&!i&&(await Promise.resolve(),this[c]())}get[g](){return this[R]}[f](e,t){return super[f]?super[f](e,t):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(n.prototype,"state",{get(){return this[g]}}),n}(function(e){return class extends e{get[r](){if(!this[z]){const e={[Y]:this};this[z]=new Proxy(e,U)}return this[z]}[l](e){if(super[l]&&super[l](e),!this[p]){const e=q(this);if(e){const s=this.attachShadow({delegatesFocus:this[t],mode:this[m]}),n=document.importNode(e.content,!0);s.append(n),this[p]=s}else this[p]=null}}get[m](){return"open"}}}(HTMLElement))),$=new Map;function _(e){if("function"==typeof e){let t;try{t=new e}catch(s){if("TypeError"!==s.name)throw s;!function(e){let t;const s=e.name&&e.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const e=/([A-Z])/g;t=s[0].replace(e,((e,t,s)=>s>0?"-"+t:t)).toLowerCase()}else t="custom-element";let n,i=$.get(t)||0;for(;n=`${t}-${i}`,customElements.get(n);i++);customElements.define(n,e),$.set(t,i+1)}(e),t=new e}return t}return document.createElement(e)}function G(e,t){const s=e.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";return(e instanceof HTMLElement||e instanceof SVGElement)&&(t instanceof HTMLElement||t instanceof SVGElement)&&(Array.prototype.forEach.call(e.attributes,(e=>{t.getAttribute(e.name)||"class"===e.name||"style"===e.name||t.setAttribute(e.name,e.value)})),Array.prototype.forEach.call(e.classList,(e=>{t.classList.add(e)})),Array.prototype.forEach.call(e.style,(s=>{t.style[s]||(t.style[s]=e.style[s])}))),t.append(...e.childNodes),s.replaceChild(t,e),t}function K(e,t){if("function"==typeof t&&e.constructor===t||"string"==typeof t&&e instanceof Element&&e.localName===t)return e;{const s=_(t);return G(e,s),s}}const X=Symbol("applyElementData"),Z=Symbol("checkSize"),J=Symbol("closestAvailableItemIndex"),Q=Symbol("contentSlot"),ee=e,te=Symbol("defaultTabIndex"),se=t,ne=Symbol("effectEndTarget"),ie=s,re=n,oe=Symbol("getItemText"),ae=Symbol("goDown"),le=Symbol("goEnd"),ce=Symbol("goFirst"),ue=Symbol("goLast"),de=Symbol("goLeft"),he=Symbol("goNext"),pe=Symbol("goPrevious"),me=Symbol("goRight"),ge=Symbol("goStart"),fe=Symbol("goToItemWithPrefix"),be=Symbol("goUp"),ye=i,we=r,ve=Symbol("inputDelegate"),xe=Symbol("itemsDelegate"),Te=Symbol("keydown"),Ee=Symbol("matchText"),Pe=Symbol("mouseenter"),Ie=Symbol("mouseleave"),Ce=o,Se=a,ke=l,Le=c,Oe=Symbol("renderDataToElement"),Ae=u,De=d,Me=Symbol("scrollTarget"),Fe=h,je=p,Re=m,He=Symbol("startEffect"),Be=g,Ne=f,We=Symbol("swipeDown"),ze=Symbol("swipeDownComplete"),Ye=Symbol("swipeLeft"),Ue=Symbol("swipeLeftTransitionEnd"),qe=Symbol("swipeRight"),Ve=Symbol("swipeRightTransitionEnd"),$e=Symbol("swipeUp"),_e=Symbol("swipeUpComplete"),Ge=Symbol("swipeStart"),Ke=Symbol("swipeTarget"),Xe=Symbol("tap"),Ze=b,Je=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:Z,closestAvailableItemIndex:J,contentSlot:Q,defaultState:ee,defaultTabIndex:te,delegatesFocus:se,effectEndTarget:ne,firstRender:ie,focusTarget:re,getItemText:oe,goDown:ae,goEnd:le,goFirst:ce,goLast:ue,goLeft:de,goNext:he,goPrevious:pe,goRight:me,goStart:ge,goToItemWithPrefix:fe,goUp:be,hasDynamicTemplate:ye,ids:we,inputDelegate:ve,itemsDelegate:xe,keydown:Te,mouseenter:Pe,mouseleave:Ie,nativeInternals:Ce,event,raiseChangeEvents:Se,render:ke,renderChanges:Le,renderDataToElement:Oe,rendered:Ae,rendering:De,scrollTarget:Me,setState:Fe,shadowRoot:je,shadowRootMode:Re,startEffect:He,state:Be,stateEffects:Ne,swipeDown:We,swipeDownComplete:ze,swipeLeft:Ye,swipeLeftTransitionEnd:Ue,swipeRight:qe,swipeRightTransitionEnd:Ve,swipeUp:$e,swipeUpComplete:_e,swipeStart:Ge,swipeTarget:Ke,tap:Xe,template:Ze,toggleSelectedFlag:Je}});const Qe=document.createElement("div");Qe.attachShadow({mode:"open",delegatesFocus:!0});const et=Qe.shadowRoot.delegatesFocus;function tt(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{composeFocus:!et})}[ke](e){super[ke]&&super[ke](e),this[ie]&&this.addEventListener("mousedown",(e=>{if(this[Be].composeFocus&&0===e.button&&e.target instanceof Element){const t=v(e.target);t&&(t.focus(),e.preventDefault())}}))}}}function st(e){return class extends e{get ariaLabel(){return this[Be].ariaLabel}set ariaLabel(e){this[Be].removingAriaAttribute||this[Fe]({ariaLabel:String(e)})}get ariaLabelledby(){return this[Be].ariaLabelledby}set ariaLabelledby(e){this[Be].removingAriaAttribute||this[Fe]({ariaLabelledby:String(e)})}get[ee](){return Object.assign(super[ee]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[ke](e){if(super[ke]&&super[ke](e),this[ie]&&this.addEventListener("focus",(()=>{this[Se]=!0;const e=it(this,this[Be]);this[Fe]({inputLabel:e}),this[Se]=!1})),e.inputLabel){const{inputLabel:e}=this[Be];e?this[ve].setAttribute("aria-label",e):this[ve].removeAttribute("aria-label")}}[Ae](e){super[Ae]&&super[Ae](e),this[ie]&&(window.requestIdleCallback||setTimeout)((()=>{const e=it(this,this[Be]);this[Fe]({inputLabel:e})}));const{ariaLabel:t,ariaLabelledby:s}=this[Be];e.ariaLabel&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",t),this[Fe]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),e.ariaLabelledby&&!this[Be].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Fe]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),e.removingAriaAttribute&&this[Be].removingAriaAttribute&&this[Fe]({removingAriaAttribute:!1})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.ariaLabel&&e.ariaLabel||t.selectedText&&e.ariaLabelledby&&this.matches(":focus-within")){const t=it(this,e);Object.assign(s,{inputLabel:t})}return s}}}function nt(e){if("selectedText"in e)return e.selectedText;if("value"in e&&"options"in e){const t=e.value,s=e.options.find((e=>e.value===t));return s?s.innerText:""}return"value"in e?e.value:e.innerText}function it(e,t){const{ariaLabel:s,ariaLabelledby:n}=t,i=e.isConnected?e.getRootNode():null;let r=null;if(n&&i)r=n.split(" ").map((s=>{const n=i.getElementById(s);return n?n===e&&null!==t.value?t.selectedText:nt(n):""})).join(" ");else if(s)r=s;else if(i){const t=e.id;if(t){const e=i.querySelector(`[for="${t}"]`);e instanceof HTMLElement&&(r=nt(e))}if(null===r){const t=e.closest("label");t&&(r=nt(t))}}return r&&(r=r.trim()),r}let rt=!1;const ot=Symbol("focusVisibleChangedListener");function at(e){return class extends e{constructor(){super(),this.addEventListener("focusout",(e=>{Promise.resolve().then((()=>{const t=e.relatedTarget||document.activeElement,s=this===t,n=x(this,t);!s&&!n&&(this[Fe]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[ot]),this[ot]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[Be].focusVisible!==rt&&this[Fe]({focusVisible:rt}),this[ot]||(this[ot]=()=>{this[Fe]({focusVisible:rt})},document.addEventListener("focusvisiblechange",this[ot]))}))}))}get[ee](){return Object.assign(super[ee]||{},{focusVisible:!1})}[ke](e){if(super[ke]&&super[ke](e),e.focusVisible){const{focusVisible:e}=this[Be];this.toggleAttribute("focus-visible",e)}}get[Ze](){const e=super[Ze]||D.html``;return e.content.append(A`
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
      `),e}}}function lt(e){if(rt!==e){rt=e;const t=new CustomEvent("focus-visible-changed",{detail:{focusVisible:rt}});document.dispatchEvent(t);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:rt}});document.dispatchEvent(s)}}function ct(e){return class extends e{get[se](){return!0}focus(e){const t=this[re];t&&t.focus(e)}get[re](){return T(this[je])}}}window.addEventListener("keydown",(()=>{lt(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{lt(!1)}),{capture:!0});const ut=Symbol("extends"),dt=Symbol("delegatedPropertySetters"),ht={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},pt={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},mt=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],gt={abort:!0,change:!0,reset:!0},ft=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],bt=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],yt=ct(V);class wt extends yt{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if(bt.indexOf(e)>=0){const t=Object.assign({},this[Be].innerAttributes,{[e]:s});this[Fe]({innerAttributes:t})}else super.attributeChangedCallback(e,t,s)}blur(){this.inner.blur()}get[ee](){return Object.assign(super[ee],{innerAttributes:{}})}get[te](){return ht[this.extends]?0:-1}get extends(){return this.constructor[ut]}get inner(){const e=this[we]&&this[we].inner;return e||console.warn("Attempted to get an inner standard element before it was instantiated."),e}static get observedAttributes(){return[...super.observedAttributes,...bt]}[ke](e){super[ke](e);const t=this.inner;if(this[ie]&&((pt[this.extends]||[]).forEach((e=>{t.addEventListener(e,(()=>{const t=new Event(e,{bubbles:gt[e]||!1});this.dispatchEvent(t)}))})),"disabled"in t&&mt.forEach((e=>{this.addEventListener(e,(e=>{t.disabled&&e.stopImmediatePropagation()}))}))),e.tabIndex&&(t.tabIndex=this[Be].tabIndex),e.innerAttributes){const{innerAttributes:e}=this[Be];for(const s in e)vt(t,s,e[s])}this.constructor[dt].forEach((s=>{if(e[s]){const e=this[Be][s];("selectionEnd"===s||"selectionStart"===s)&&null===e||(t[s]=e)}}))}[Ae](e){if(super[Ae](e),e.disabled){const{disabled:e}=this[Be];void 0!==e&&S(this,"disabled",e)}}get[Ze](){const e=ft.includes(this.extends)?"block":"inline-block",t=this.extends;return D.html`
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
    `}static wrap(e){class t extends wt{}t[ut]=e;const s=document.createElement(e);return function(e,t){const s=Object.getOwnPropertyNames(t);e[dt]=[],s.forEach((s=>{const n=Object.getOwnPropertyDescriptor(t,s);if(!n)return;const i=function(e,t){if("function"==typeof t.value){if("constructor"!==e)return function(e,t){return{configurable:t.configurable,enumerable:t.enumerable,value:function(...t){this.inner[e](...t)},writable:t.writable}}(e,t)}else if("function"==typeof t.get||"function"==typeof t.set)return function(e,t){const s={configurable:t.configurable,enumerable:t.enumerable};return t.get&&(s.get=function(){return function(e,t){return e[Be][t]||e[je]&&e.inner[t]}(this,e)}),t.set&&(s.set=function(t){!function(e,t,s){e[Be][t]!==s&&e[Fe]({[t]:s})}(this,e,t)}),t.writable&&(s.writable=t.writable),s}(e,t);return null}(s,n);i&&(Object.defineProperty(e.prototype,s,i),i.set&&e[dt].push(s))}))}(t,Object.getPrototypeOf(s)),t}}function vt(e,t,s){k[t]?"string"==typeof s?e.setAttribute(t,""):null===s&&e.removeAttribute(t):null!=s?e.setAttribute(t,s.toString()):e.removeAttribute(t)}const xt=wt,Tt=tt(st(at(xt.wrap("button")))),Et=class extends Tt{get[ee](){return Object.assign(super[ee],{role:"button"})}get[ve](){return this[we].inner}[Xe](){const e=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(e)}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},Pt=Symbol("wrap");function It(e){return class extends e{get arrowButtonOverlap(){return this[Be].arrowButtonOverlap}set arrowButtonOverlap(e){this[Fe]({arrowButtonOverlap:e})}get arrowButtonPartType(){return this[Be].arrowButtonPartType}set arrowButtonPartType(e){this[Fe]({arrowButtonPartType:e})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[pe]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[he]()}attributeChangedCallback(e,t,s){"arrow-button-overlap"===e?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===e?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{arrowButtonOverlap:!0,arrowButtonPartType:Et,orientation:"horizontal",showArrowButtons:!0})}[ke](e){if(e.arrowButtonPartType){const e=this[we].arrowButtonPrevious;e instanceof HTMLElement&&E(e,null);const t=this[we].arrowButtonNext;t instanceof HTMLElement&&E(t,null)}if(super[ke]&&super[ke](e),St(this[je],this[Be],e),e.arrowButtonPartType){const e=this,t=this[we].arrowButtonPrevious;t instanceof HTMLElement&&E(t,e);const s=Ct(this,(()=>this.arrowButtonPrevious()));t.addEventListener("mousedown",s);const n=this[we].arrowButtonNext;n instanceof HTMLElement&&E(n,e);const i=Ct(this,(()=>this.arrowButtonNext()));n.addEventListener("mousedown",i)}const{arrowButtonOverlap:t,canGoNext:s,canGoPrevious:n,orientation:i,rightToLeft:r}=this[Be],o="vertical"===i,a=this[we].arrowButtonPrevious,l=this[we].arrowButtonNext;if(e.arrowButtonOverlap||e.orientation||e.rightToLeft){this[we].arrowDirection.style.flexDirection=o?"column":"row";const e={bottom:null,left:null,right:null,top:null};let s,n;t?Object.assign(e,{position:"absolute","z-index":1}):Object.assign(e,{position:null,"z-index":null}),t&&(o?(Object.assign(e,{left:0,right:0}),s={top:0},n={bottom:0}):(Object.assign(e,{bottom:0,top:0}),r?(s={right:0},n={left:0}):(s={left:0},n={right:0}))),Object.assign(a.style,e,s),Object.assign(l.style,e,n)}if(e.canGoNext&&null!==s&&(l.disabled=!s),e.canGoPrevious&&null!==n&&(a.disabled=!n),e.showArrowButtons){const e=this[Be].showArrowButtons?null:"none";a.style.display=e,l.style.display=e}}get showArrowButtons(){return this[Be].showArrowButtons}set showArrowButtons(e){this[Fe]({showArrowButtons:e})}[Pt](e){const t=A`
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
      `;St(t,this[Be]);const s=t.getElementById("arrowDirectionContainer");s&&(e.replaceWith(t),s.append(e))}}}function Ct(e,t){return async function(s){0===s.button&&(e[Se]=!0,t()&&s.stopPropagation(),await Promise.resolve(),e[Se]=!1)}}function St(e,t,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=t,n=e.getElementById("arrowButtonPrevious");n&&K(n,s);const i=e.getElementById("arrowButtonNext");i&&K(i,s)}}It.wrap=Pt;const kt=It,Lt={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},Ot=864e5;function At(e,t){const s=e.includes("-ca-")?"":"-ca-gregory",n=e.includes("-nu-")?"":"-nu-latn",i=`${e}${s||n?"-u":""}${s}${n}`;return new Intl.DateTimeFormat(i,t)}function Dt(e,t){return null===e&&null===t||null!==e&&null!==t&&e.getTime()===t.getTime()}function Mt(e,t){const s=Ft(t);return(e.getDay()-s+7)%7}function Ft(e){const t=qt(e),s=Lt.firstDay[t];return void 0!==s?s:Lt.firstDay["001"]}function jt(e){const t=Rt(e);return t.setDate(1),t}function Rt(e){const t=new Date(e.getTime());return t.setHours(0),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Ht(e){const t=new Date(e.getTime());return t.setHours(12),t.setMinutes(0),t.setSeconds(0),t.setMilliseconds(0),t}function Bt(e,t){const s=Ht(e);return s.setDate(s.getDate()+t),Ut(e,s),s}function Nt(e,t){const s=Ht(e);return s.setMonth(e.getMonth()+t),Ut(e,s),s}function Wt(){return Rt(new Date)}function zt(e){const t=qt(e),s=Lt.weekendEnd[t];return void 0!==s?s:Lt.weekendEnd["001"]}function Yt(e){const t=qt(e),s=Lt.weekendStart[t];return void 0!==s?s:Lt.weekendStart["001"]}function Ut(e,t){t.setHours(e.getHours()),t.setMinutes(e.getMinutes()),t.setSeconds(e.getSeconds()),t.setMilliseconds(e.getMilliseconds())}function qt(e){const t=e?e.split("-"):null;return t?t[1]:"001"}function Vt(e){return class extends e{attributeChangedCallback(e,t,s){"date"===e?this.date=new Date(s):super.attributeChangedCallback(e,t,s)}get date(){return this[Be].date}set date(e){Dt(e,this[Be].date)||this[Fe]({date:e})}get[ee](){return Object.assign(super[ee]||{},{date:null,locale:navigator.language})}get locale(){return this[Be].locale}set locale(e){this[Fe]({locale:String(e)})}[Ae](e){if(super[Ae]&&super[Ae](e),e.date&&this[Se]){const e=this[Be].date,t=new CustomEvent("date-changed",{bubbles:!0,detail:{date:e}});this.dispatchEvent(t);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:e}});this.dispatchEvent(s)}}}}function $t(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}get[ee](){return Object.assign(super[ee]||{},{selected:!1})}[ke](e){if(super[ke](e),e.selected){const{selected:e}=this[Be];S(this,"selected",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.selected){const{selected:e}=this[Be],t=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:e}});this.dispatchEvent(s)}}get selected(){return this[Be].selected}set selected(e){this[Fe]({selected:e})}}}const _t=Vt($t(V)),Gt=class extends _t{get[ee](){return Object.assign(super[ee],{date:Wt(),outsideRange:!1})}[ke](e){super[ke](e);const{date:t}=this[Be];if(e.date){const e=Wt(),s=t.getDay(),n=t.getDate(),i=Bt(t,1),r=Math.round(t.getTime()-e.getTime())/Ot;S(this,"alternate-month",Math.abs(t.getMonth()-e.getMonth())%2==1),S(this,"first-day-of-month",1===n),S(this,"first-week",n<=7),S(this,"future",t>e),S(this,"last-day-of-month",t.getMonth()!==i.getMonth()),S(this,"past",t<e),S(this,"sunday",0===s),S(this,"monday",1===s),S(this,"tuesday",2===s),S(this,"wednesday",3===s),S(this,"thursday",4===s),S(this,"friday",5===s),S(this,"saturday",6===s),S(this,"today",0===r),this[we].day.textContent=n.toString()}if(e.date||e.locale){const e=t.getDay(),{locale:s}=this[Be],n=e===Yt(s)||e===zt(s);S(this,"weekday",!n),S(this,"weekend",n)}e.outsideRange&&S(this,"outside-range",this[Be].outsideRange)}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Fe]({outsideRange:e})}get[Ze](){return D.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Kt=$t(Et),Xt=Vt(class extends Kt{}),Zt=class extends Xt{get[ee](){return Object.assign(super[ee],{date:Wt(),dayPartType:Gt,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get outsideRange(){return this[Be].outsideRange}set outsideRange(e){this[Fe]({outsideRange:e})}[ke](e){if(super[ke](e),e.dayPartType){const{dayPartType:e}=this[Be];K(this[we].day,e)}const t=this[we].day;(e.dayPartType||e.date)&&(t.date=this[Be].date),(e.dayPartType||e.locale)&&(t.locale=this[Be].locale),(e.dayPartType||e.outsideRange)&&(t.outsideRange=this[Be].outsideRange),(e.dayPartType||e.selected)&&(t.selected=this[Be].selected)}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");if(t){const e=_(this[Be].dayPartType);e.id="day",t.replaceWith(e)}return e.content.append(A`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),e}},Jt=class extends V{get[ee](){return Object.assign(super[ee],{format:"short",locale:navigator.language})}get format(){return this[Be].format}set format(e){this[Fe]({format:e})}get locale(){return this[Be].locale}set locale(e){this[Fe]({locale:String(e)})}[ke](e){if(super[ke](e),e.format||e.locale){const{format:e,locale:t}=this[Be],s=At(t,{weekday:e}),n=Ft(t),i=Yt(t),r=zt(t),o=new Date(2017,0,1),a=this[je].querySelectorAll('[part~="day-name"]');for(let e=0;e<=6;e++){const t=(n+e)%7;o.setDate(t+1);const l=t===i||t===r,c=a[e];c.toggleAttribute("weekday",!l),c.toggleAttribute("weekend",l),c.textContent=s.format(o)}}}get[Ze](){return D.html`
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
    `}},Qt=Vt(V),es=class extends Qt{attributeChangedCallback(e,t,s){"start-date"===e?this.startDate=new Date(s):super.attributeChangedCallback(e,t,s)}dayElementForDate(e){return(this.days||[]).find((t=>Dt(t.date,e)))}get dayCount(){return this[Be].dayCount}set dayCount(e){this[Fe]({dayCount:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get days(){return this[Be].days}get[ee](){const e=Wt();return Object.assign(super[ee],{date:e,dayCount:1,dayPartType:Gt,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:e})}[ke](e){if(super[ke](e),e.days&&L(this[we].dayContainer,this[Be].days),e.date||e.locale||e.showSelectedDay){const e=this[Be].showSelectedDay,{date:t}=this[Be],s=t.getDate(),n=t.getMonth(),i=t.getFullYear();(this.days||[]).forEach((t=>{const r=t.date,o=e&&r.getDate()===s&&r.getMonth()===n&&r.getFullYear()===i;t.toggleAttribute("selected",o)}))}if(e.dayCount||e.startDate){const{dayCount:e,startDate:t}=this[Be],s=Bt(t,e);(this[Be].days||[]).forEach((e=>{if("outsideRange"in e){const n=e.date.getTime(),i=n<t.getTime()||n>=s.getTime();e.outsideRange=i}}))}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Fe]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Fe]({showSelectedDay:e})}get startDate(){return this[Be].startDate}set startDate(e){Dt(this[Be].startDate,e)||this[Fe]({startDate:e})}[Ne](e,t){const s=super[Ne](e,t);if(t.dayCount||t.dayPartType||t.locale||t.showCompleteWeeks||t.startDate){const n=function(e,t){const{dayCount:s,dayPartType:n,locale:i,showCompleteWeeks:r,startDate:o}=e,a=r?function(e,t){return Rt(Bt(e,-Mt(e,t)))}(o,i):Rt(o);let l;if(r){c=a,u=function(e,t){return Rt(Bt(e,6-Mt(e,t)))}(Bt(o,s-1),i),l=Math.round((u.getTime()-c.getTime())/Ot)+1}else l=s;var c,u;let d=e.days?e.days.slice():[],h=a;for(let e=0;e<l;e++){const s=t||e>=d.length,r=s?_(n):d[e];r.date=new Date(h.getTime()),r.locale=i,"part"in r&&(r.part="day"),r.style.gridColumnStart="",s&&(d[e]=r),h=Bt(h,1)}l<d.length&&(d=d.slice(0,l));const p=d[0];if(p&&!r){const t=Mt(p.date,e.locale);p.style.gridColumnStart=t+1}return Object.freeze(d),d}(e,t.dayPartType);Object.assign(s,{days:n})}return s}get[Ze](){return D.html`
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
    `}},ts=Vt(V),ss=class extends ts{get[ee](){return Object.assign(super[ee],{date:Wt(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Fe]({monthFormat:e})}[ke](e){if(super[ke](e),e.date||e.locale||e.monthFormat||e.yearFormat){const{date:e,locale:t,monthFormat:s,yearFormat:n}=this[Be],i={};s&&(i.month=s),n&&(i.year=n);const r=At(t,i);this[we].formatted.textContent=r.format(e)}}get[Ze](){return D.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Fe]({yearFormat:e})}},ns=Vt(V);function is(e,t,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=t,n=e.getElementById("dayNamesHeader");n&&K(n,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=t,n=e.getElementById("monthYearHeader");n&&K(n,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=t,n=e.getElementById("monthDays");n&&K(n,s)}}function rs(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}checkValidity(){return this[Ce].checkValidity()}get[ee](){return Object.assign(super[ee]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[Ce]}static get formAssociated(){return!0}get form(){return this[Ce].form}get name(){return this[Be]?this[Be].name:""}set name(t){const s=String(t);"name"in e.prototype&&(super.name=s),this[Fe]({name:s})}[ke](e){if(super[ke]&&super[ke](e),e.name){const{name:e}=this[Be];e?this.setAttribute("name",e):this.removeAttribute("name")}if(this[Ce]&&this[Ce].setValidity&&(e.valid||e.validationMessage)){const{valid:e,validationMessage:t}=this[Be];e?this[Ce].setValidity({}):this[Ce].setValidity({customError:!0},t)}}[Ae](e){super[Ae]&&super[Ae](e),e.value&&this[Ce]&&this[Ce].setFormValue(this[Be].value,this[Be])}reportValidity(){return this[Ce].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[Be].validationMessage}get validity(){return this[Ce].validity}get willValidate(){return this[Ce].willValidate}}}function os(e){return class extends e{[ae](){if(super[ae])return super[ae]()}[le](){if(super[le])return super[le]()}[de](){if(super[de])return super[de]()}[me](){if(super[me])return super[me]()}[ge](){if(super[ge])return super[ge]()}[be](){if(super[be])return super[be]()}[Te](e){let t=!1;const s=this[Be].orientation||"both",n="horizontal"===s||"both"===s,i="vertical"===s||"both"===s;switch(e.key){case"ArrowDown":i&&(t=e.altKey?this[le]():this[ae]());break;case"ArrowLeft":!n||e.metaKey||e.altKey||(t=this[de]());break;case"ArrowRight":!n||e.metaKey||e.altKey||(t=this[me]());break;case"ArrowUp":i&&(t=e.altKey?this[ge]():this[be]());break;case"End":t=this[le]();break;case"Home":t=this[ge]()}return t||super[Te]&&super[Te](e)||!1}}}function as(e){return class extends e{constructor(){super(),this.addEventListener("keydown",(async e=>{this[Se]=!0,this[Be].focusVisible||this[Fe]({focusVisible:!0}),this[Te](e)&&(e.preventDefault(),e.stopImmediatePropagation()),await Promise.resolve(),this[Se]=!1}))}attributeChangedCallback(e,t,s){if("tabindex"===e){let e;null===s?e=-1:(e=Number(s),isNaN(e)&&(e=this[te]?this[te]:0)),this.tabIndex=e}else super.attributeChangedCallback(e,t,s)}get[ee](){const e=this[se]?-1:0;return Object.assign(super[ee]||{},{tabIndex:e})}[Te](e){return!!super[Te]&&super[Te](e)}[ke](e){super[ke]&&super[ke](e),e.tabIndex&&(this.tabIndex=this[Be].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(e){super.tabIndex!==e&&(super.tabIndex=e),this[De]||this[Fe]({tabIndex:e})}}}function ls(e){return class extends e{connectedCallback(){const e="rtl"===getComputedStyle(this).direction;this[Fe]({rightToLeft:e}),super.connectedCallback()}}}const cs=kt(Vt(at(rs(os(as(ls(class extends ns{dayElementForDate(e){const t=this[we].monthDays;return t&&"dayElementForDate"in t&&t.dayElementForDate(e)}get dayNamesHeaderPartType(){return this[Be].dayNamesHeaderPartType}set dayNamesHeaderPartType(e){this[Fe]({dayNamesHeaderPartType:e})}get dayPartType(){return this[Be].dayPartType}set dayPartType(e){this[Fe]({dayPartType:e})}get days(){return this[je]?this[we].monthDays.days:[]}get daysOfWeekFormat(){return this[Be].daysOfWeekFormat}set daysOfWeekFormat(e){this[Fe]({daysOfWeekFormat:e})}get[ee](){return Object.assign(super[ee],{date:Wt(),dayNamesHeaderPartType:Jt,dayPartType:Gt,daysOfWeekFormat:"short",monthDaysPartType:es,monthFormat:"long",monthYearHeaderPartType:ss,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[Be].monthFormat}set monthFormat(e){this[Fe]({monthFormat:e})}get monthDaysPartType(){return this[Be].monthDaysPartType}set monthDaysPartType(e){this[Fe]({monthDaysPartType:e})}get monthYearHeaderPartType(){return this[Be].monthYearHeaderPartType}set monthYearHeaderPartType(e){this[Fe]({monthYearHeaderPartType:e})}[ke](e){if(super[ke](e),is(this[je],this[Be],e),(e.dayPartType||e.monthDaysPartType)&&(this[we].monthDays.dayPartType=this[Be].dayPartType),e.locale||e.monthDaysPartType||e.monthYearHeaderPartType||e.dayNamesHeaderPartType){const e=this[Be].locale;this[we].monthDays.locale=e,this[we].monthYearHeader.locale=e,this[we].dayNamesHeader.locale=e}if(e.date||e.monthDaysPartType){const{date:e}=this[Be];if(e){const t=jt(e),s=function(e){const t=jt(e);return t.setMonth(t.getMonth()+1),t.setDate(t.getDate()-1),t}(e).getDate();Object.assign(this[we].monthDays,{date:e,dayCount:s,startDate:t}),this[we].monthYearHeader.date=jt(e)}}if(e.daysOfWeekFormat||e.dayNamesHeaderPartType){const{daysOfWeekFormat:e}=this[Be];this[we].dayNamesHeader.format=e}if(e.showCompleteWeeks||e.monthDaysPartType){const{showCompleteWeeks:e}=this[Be];this[we].monthDays.showCompleteWeeks=e}if(e.showSelectedDay||e.monthDaysPartType){const{showSelectedDay:e}=this[Be];this[we].monthDays.showSelectedDay=e}if(e.monthFormat||e.monthYearHeaderPartType){const{monthFormat:e}=this[Be];this[we].monthYearHeader.monthFormat=e}if(e.yearFormat||e.monthYearHeaderPartType){const{yearFormat:e}=this[Be];this[we].monthYearHeader.yearFormat=e}}get showCompleteWeeks(){return this[Be].showCompleteWeeks}set showCompleteWeeks(e){this[Fe]({showCompleteWeeks:e})}get showSelectedDay(){return this[Be].showSelectedDay}set showSelectedDay(e){this[Fe]({showSelectedDay:e})}get[Ze](){const e=D.html`
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
    `;return is(e.content,this[Be]),e}get yearFormat(){return this[Be].yearFormat}set yearFormat(e){this[Fe]({yearFormat:e})}}))))))),us=class extends cs{constructor(){super(),this.addEventListener("mousedown",(e=>{if(0!==e.button)return;this[Se]=!0;const t=e.composedPath()[0];if(t instanceof Node){const e=this.days,s=e[P(e,t)];s&&(this.date=s.date)}this[Se]=!1})),E(this,this)}arrowButtonNext(){const e=this[Be].date||Wt();return this[Fe]({date:Nt(e,1)}),!0}arrowButtonPrevious(){const e=this[Be].date||Wt();return this[Fe]({date:Nt(e,-1)}),!0}get[ee](){return Object.assign(super[ee],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Wt(),dayPartType:Zt,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[Te](e){let t=!1;switch(e.key){case"Home":this[Fe]({date:Wt()}),t=!0;break;case"PageDown":this[Fe]({date:Nt(this[Be].date,1)}),t=!0;break;case"PageUp":this[Fe]({date:Nt(this[Be].date,-1)}),t=!0}return t||super[Te]&&super[Te](e)}[ae](){return super[ae]&&super[ae](),this[Fe]({date:Bt(this[Be].date,7)}),!0}[de](){return super[de]&&super[de](),this[Fe]({date:Bt(this[Be].date,-1)}),!0}[me](){return super[me]&&super[me](),this[Fe]({date:Bt(this[Be].date,1)}),!0}[be](){return super[be]&&super[be](),this[Fe]({date:Bt(this[Be].date,-7)}),!0}[Ne](e,t){const s=super[Ne](e,t);return t.date&&Object.assign(s,{value:e.date?e.date.toString():""}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("#monthYearHeader");this[kt.wrap](t);const s=D.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return e.content.append(s.content),e}get value(){return this.date}set value(e){this.date=e}},ds=new Set;function hs(e){return class extends e{attributeChangedCallback(e,t,s){if("dark"===e){const t=w(e,s);this.dark!==t&&(this.dark=t)}else super.attributeChangedCallback(e,t,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),ds.delete(this)}get dark(){return this[Be].dark}set dark(e){this[Fe]({dark:e})}get[ee](){return Object.assign(super[ee]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[Be].detectDarkMode}set detectDarkMode(e){"auto"!==e&&"off"!==e||this[Fe]({detectDarkMode:e})}[ke](e){if(super[ke]&&super[ke](e),e.dark){const{dark:e}=this[Be];S(this,"dark",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.detectDarkMode){const{detectDarkMode:e}=this[Be];"auto"===e?(ds.add(this),ps(this)):ds.delete(this)}}}}function ps(e){const t=function(e){const t=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(e);return t?{r:t[1],g:t[2],b:t[3]}:null}(ms(e));if(t){const s=function(e){const t=e.r/255,s=e.g/255,n=e.b/255,i=Math.max(t,s,n),r=Math.min(t,s,n);let o=0,a=0,l=(i+r)/2;const c=i-r;if(0!==c){switch(a=l>.5?c/(2-c):c/(i+r),i){case t:o=(s-n)/c+(s<n?6:0);break;case s:o=(n-t)/c+2;break;case n:o=(t-s)/c+4}o/=6}return{h:o,s:a,l}}(t).l<.5;e[Fe]({dark:s})}}function ms(e){const t="rgb(255,255,255)";if(e instanceof Document)return t;const s=getComputedStyle(e).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(e.assignedSlot)return ms(e.assignedSlot);const n=e.parentNode;return n instanceof ShadowRoot?ms(n.host):n instanceof Element?ms(n):t}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{ds.forEach((e=>{ps(e)}))}));class gs extends(function(e){return class extends e{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}}}(Et)){}const fs=gs,bs=hs(fs),ys=class extends bs{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},ws=class extends Gt{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},vs=class extends Zt{get[ee](){return Object.assign(super[ee],{dayPartType:ws})}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},xs=class extends Jt{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},Ts=class extends ss{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),e}};class Es extends(hs(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{arrowButtonPartType:ys})}[ke](e){if(super[ke](e),e.orientation||e.rightToLeft){const{orientation:e,rightToLeft:t}=this[Be],s="vertical"===e?"rotate(90deg)":t?"rotateZ(180deg)":"";this[we].arrowIconPrevious&&(this[we].arrowIconPrevious.style.transform=s),this[we].arrowIconNext&&(this[we].arrowIconNext.style.transform=s)}if(e.dark){const{dark:e}=this[Be],t=this[we].arrowButtonPrevious,s=this[we].arrowButtonNext;"dark"in t&&(t.dark=e),"dark"in s&&(s.dark=e)}}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="arrowButtonPrevious"]');t&&t.append(A`
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
          `),e}}}(us))){get[ee](){return Object.assign(super[ee],{dayNamesHeaderPartType:xs,dayPartType:vs,monthYearHeaderPartType:Ts})}}const Ps=Es;customElements.define("elix-calendar-month-navigator",class extends Ps{});const Is=Symbol("generatedId");let Cs=0;const Ss={a:"link",article:"region",button:"button",h1:"sectionhead",h2:"sectionhead",h3:"sectionhead",h4:"sectionhead",h5:"sectionhead",h6:"sectionhead",hr:"sectionhead",iframe:"region",link:"link",menu:"menu",ol:"list",option:"option",output:"liveregion",progress:"progressbar",select:"select",table:"table",td:"td",textarea:"textbox",th:"th",ul:"list"};function ks(e){let t=e.id||e[Is];return t||(t="_id"+Cs++,e[Is]=t),t}function Ls(e){return class extends e{attributeChangedCallback(e,t,s){if("current-index"===e)this.currentIndex=Number(s);else if("current-item-required"===e){const t=w(e,s);this.currentItemRequired!==t&&(this.currentItemRequired=t)}else if("cursor-operations-wrap"===e){const t=w(e,s);this.cursorOperationsWrap!==t&&(this.cursorOperationsWrap=t)}else super.attributeChangedCallback(e,t,s)}get currentIndex(){const{items:e,currentIndex:t}=this[Be];return e&&e.length>0?t:-1}set currentIndex(e){isNaN(e)||this[Fe]({currentIndex:e})}get currentItem(){const{items:e,currentIndex:t}=this[Be];return e&&e[t]}set currentItem(e){const{items:t}=this[Be];if(!t)return;const s=t.indexOf(e);s>=0&&this[Fe]({currentIndex:s})}get currentItemRequired(){return this[Be].currentItemRequired}set currentItemRequired(e){this[Fe]({currentItemRequired:e})}get cursorOperationsWrap(){return this[Be].cursorOperationsWrap}set cursorOperationsWrap(e){this[Fe]({cursorOperationsWrap:e})}goFirst(){return super.goFirst&&super.goFirst(),this[ce]()}goLast(){return super.goLast&&super.goLast(),this[ue]()}goNext(){return super.goNext&&super.goNext(),this[he]()}goPrevious(){return super.goPrevious&&super.goPrevious(),this[pe]()}[Ae](e){if(super[Ae]&&super[Ae](e),e.currentIndex&&this[Se]){const{currentIndex:e}=this[Be],t=new CustomEvent("current-index-changed",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("currentindexchange",{bubbles:!0,detail:{currentIndex:e}});this.dispatchEvent(s)}}}}function Os(e,t,s){if(!(e instanceof Node))return!1;for(const n of C(e))if(n instanceof HTMLElement){const e=getComputedStyle(n),i="vertical"===t;if(i&&("scroll"===e.overflowY||"auto"===e.overflowY)||!i&&("scroll"===e.overflowX||"auto"===e.overflowX)){const e=i?"scrollTop":"scrollLeft";if(!s&&n[e]>0)return!0;const t=i?"clientHeight":"clientWidth",r=n[i?"scrollHeight":"scrollWidth"]-n[t];if(s&&n[e]<r)return!0}}return!1}function As(e){const t=e[je],s=t&&t.querySelector("slot:not([name])");return s&&s.parentNode instanceof Element&&function(e){for(const t of C(e))if(t instanceof HTMLElement&&Ds(t))return t;return null}(s.parentNode)||e}function Ds(e){const t=getComputedStyle(e),s=t.overflowX,n=t.overflowY;return"scroll"===s||"auto"===s||"scroll"===n||"auto"===n}function Ms(e){return class extends e{[Ae](e){super[Ae]&&super[Ae](e),e.currentItem&&this.scrollCurrentItemIntoView()}scrollCurrentItemIntoView(){super.scrollCurrentItemIntoView&&super.scrollCurrentItemIntoView();const{currentItem:e,items:t}=this[Be];if(!e||!t)return;const s=this[Me].getBoundingClientRect(),n=e.getBoundingClientRect(),i=n.bottom-s.bottom,r=n.left-s.left,o=n.right-s.right,a=n.top-s.top,l=this[Be].orientation||"both";"horizontal"!==l&&"both"!==l||(o>0?this[Me].scrollLeft+=o:r<0&&(this[Me].scrollLeft+=Math.ceil(r))),"vertical"!==l&&"both"!==l||(i>0?this[Me].scrollTop+=i:a<0&&(this[Me].scrollTop+=Math.ceil(a)))}get[Me](){return super[Me]||As(this)}}}function Fs(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{canGoDown:null,canGoLeft:null,canGoRight:null,canGoUp:null})}[ae](){return super[ae]&&super[ae](),this[he]()}[le](){return super[le]&&super[le](),this[ue]()}[de](){return super[de]&&super[de](),this[Be]&&this[Be].rightToLeft?this[he]():this[pe]()}[me](){return super[me]&&super[me](),this[Be]&&this[Be].rightToLeft?this[pe]():this[he]()}[ge](){return super[ge]&&super[ge](),this[ce]()}[be](){return super[be]&&super[be](),this[pe]()}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.canGoNext||t.canGoPrevious||t.languageDirection||t.orientation||t.rightToLeft){const{canGoNext:t,canGoPrevious:n,orientation:i,rightToLeft:r}=e,o="horizontal"===i||"both"===i,a="vertical"===i||"both"===i,l=a&&t,c=!!o&&(r?t:n),u=!!o&&(r?n:t),d=a&&n;Object.assign(s,{canGoDown:l,canGoLeft:c,canGoRight:u,canGoUp:d})}return s}}}function js(e){return class extends e{get items(){return this[Be]?this[Be].items:null}[Ae](e){if(super[Ae]&&super[Ae](e),!this[ie]&&e.items&&this[Se]){const e=new CustomEvent("items-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("itemschange",{bubbles:!0});this.dispatchEvent(t)}}}}function Rs(e){return class extends e{[J](e,t={}){const s=void 0!==t.direction?t.direction:1,n=void 0!==t.index?t.index:e.currentIndex,i=void 0!==t.wrap?t.wrap:e.cursorOperationsWrap,{items:r}=e,o=r?r.length:0;if(0===o)return-1;if(i){let t=(n%o+o)%o;const i=((t-s)%o+o)%o;for(;t!==i;){if(!e.availableItemFlags||e.availableItemFlags[t])return t;t=((t+s)%o+o)%o}}else for(let t=n;t>=0&&t<o;t+=s)if(!e.availableItemFlags||e.availableItemFlags[t])return t;return-1}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,desiredCurrentIndex:null,currentItem:null,currentItemRequired:!1,cursorOperationsWrap:!1})}[ce](){return super[ce]&&super[ce](),Hs(this,0,1)}[ue](){return super[ue]&&super[ue](),Hs(this,this[Be].items.length-1,-1)}[he](){super[he]&&super[he]();const{currentIndex:e,items:t}=this[Be];return Hs(this,e<0&&t?0:e+1,1)}[pe](){super[pe]&&super[pe]();const{currentIndex:e,items:t}=this[Be];return Hs(this,e<0&&t?t.length-1:e-1,-1)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.availableItemFlags||t.items||t.currentIndex||t.currentItemRequired){const{currentIndex:n,desiredCurrentIndex:i,currentItem:r,currentItemRequired:o,items:a}=e,l=a?a.length:0;let c,u=i;if(t.items&&!t.currentIndex&&r&&l>0&&a[n]!==r){const e=a.indexOf(r);e>=0&&(u=e)}else t.currentIndex&&(n<0&&null!==r||n>=0&&(0===l||a[n]!==r)||null===i)&&(u=n);o&&u<0&&(u=0),u<0?(u=-1,c=-1):0===l?c=-1:(c=Math.max(Math.min(l-1,u),0),c=this[J](e,{direction:1,index:c,wrap:!1}),c<0&&(c=this[J](e,{direction:-1,index:c-1,wrap:!1})));const d=a&&a[c]||null;Object.assign(s,{currentIndex:c,desiredCurrentIndex:u,currentItem:d})}return s}}}function Hs(e,t,s){const n=e[J](e[Be],{direction:s,index:t});if(n<0)return!1;const i=e[Be].currentIndex!==n;return i&&e[Fe]({currentIndex:n}),i}const Bs=["applet","basefont","embed","font","frame","frameset","isindex","keygen","link","multicol","nextid","noscript","object","param","script","style","template","noembed"];function Ns(e){return e.getAttribute("aria-label")||e.getAttribute("alt")||e.innerText||e.textContent||""}function Ws(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{texts:null})}[oe](e){return super[oe]?super[oe](e):Ns(e)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items){const{items:t}=e,n=function(e,t){return e?Array.from(e,(e=>t(e))):null}(t,this[oe]);n&&(Object.freeze(n),Object.assign(s,{texts:n}))}return s}}}function zs(e){return class extends e{[Te](e){let t=!1;if("horizontal"!==this.orientation)switch(e.key){case"PageDown":t=this.pageDown();break;case"PageUp":t=this.pageUp()}return t||super[Te]&&super[Te](e)}get orientation(){return super.orientation||this[Be]&&this[Be].orientation||"both"}pageDown(){return super.pageDown&&super.pageDown(),Us(this,!0)}pageUp(){return super.pageUp&&super.pageUp(),Us(this,!1)}get[Me](){return super[Me]||As(this)}}}function Ys(e,t,s){const n=e[Be].items,i=s?0:n.length-1,r=s?n.length:0,o=s?1:-1;let a,l,c=null;const{availableItemFlags:u}=e[Be];for(a=i;a!==r;a+=o)if((!u||u[a])&&(l=n[a].getBoundingClientRect(),l.top<=t&&t<=l.bottom)){c=n[a];break}if(!c||!l)return null;const d=getComputedStyle(c),h=d.paddingTop?parseFloat(d.paddingTop):0,p=d.paddingBottom?parseFloat(d.paddingBottom):0,m=l.top+h,g=m+c.clientHeight-h-p;return s&&m<=t||!s&&g>=t?a:a-o}function Us(e,t){const s=e[Be].items,n=e[Be].currentIndex,i=e[Me].getBoundingClientRect(),r=Ys(e,t?i.bottom:i.top,t);let o;if(r&&n===r){const i=s[n].getBoundingClientRect(),r=e[Me].clientHeight;o=Ys(e,t?i.bottom+r:i.top-r,t)}else o=r;if(!o){const n=t?s.length-1:0;o=e[J]?e[J](e[Be],{direction:t?-1:1,index:n}):n}const a=o!==n;if(a){const t=e[Se];e[Se]=!0,e[Fe]({currentIndex:o}),e[Se]=t}return a}const qs=Symbol("typedPrefix"),Vs=Symbol("prefixTimeout");function $s(e){return class extends e{constructor(){super(),Gs(this)}[fe](e){if(super[fe]&&super[fe](e),null==e||0===e.length)return!1;const t=e.toLowerCase(),s=this[Be].texts.findIndex((s=>s.substr(0,e.length).toLowerCase()===t));if(s>=0){const e=this[Be].currentIndex;return this[Fe]({currentIndex:s}),this[Be].currentIndex!==e}return!1}[Te](e){let t;switch(e.key){case"Backspace":!function(e){const t=e,s=t[qs]?t[qs].length:0;s>0&&(t[qs]=t[qs].substr(0,s-1)),e[fe](t[qs]),Ks(e)}(this),t=!0;break;case"Escape":Gs(this);break;default:e.ctrlKey||e.metaKey||e.altKey||1!==e.key.length||function(e,t){const s=e,n=s[qs]||"";s[qs]=n+t,e[fe](s[qs]),Ks(e)}(this,e.key)}return t||super[Te]&&super[Te](e)}}}function _s(e){const t=e;t[Vs]&&(clearTimeout(t[Vs]),t[Vs]=!1)}function Gs(e){e[qs]="",_s(e)}function Ks(e){_s(e),e[Vs]=setTimeout((()=>{Gs(e)}),1e3)}function Xs(e){return class extends e{get[Q](){const e=this[je]&&this[je].querySelector("slot:not([name])");return this[je]&&e||console.warn(`SlotContentMixin expects ${this.constructor.name} to define a shadow tree that includes a default (unnamed) slot.\nSee https://elix.org/documentation/SlotContentMixin.`),e}get[ee](){return Object.assign(super[ee]||{},{content:null})}[Ae](e){if(super[Ae]&&super[Ae](e),this[ie]){const e=this[Q];e&&e.addEventListener("slotchange",(async()=>{this[Se]=!0;const t=e.assignedNodes({flatten:!0});Object.freeze(t),this[Fe]({content:t}),await Promise.resolve(),this[Se]=!1}))}}}}function Zs(e){return function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{items:null})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.content){const t=e.content,n=t?Array.prototype.filter.call(t,(e=>{return(t=e)instanceof Element&&(!t.localName||Bs.indexOf(t.localName)<0);var t})):null;n&&Object.freeze(n),Object.assign(s,{items:n})}return s}}}(Xs(e))}function Js(e){return class extends e{constructor(){super(),this.addEventListener("mousedown",(e=>{0===e.button&&(this[Se]=!0,this[Xe](e),this[Se]=!1)}))}[ke](e){super[ke]&&super[ke](e),this[ie]&&Object.assign(this.style,{touchAction:"manipulation",mozUserSelect:"none",msUserSelect:"none",webkitUserSelect:"none",userSelect:"none"})}[Xe](e){const t=e.composedPath?e.composedPath()[0]:e.target,{items:s,currentItemRequired:n}=this[Be];if(s&&t instanceof Node){const i=P(s,t),r=i>=0?s[i]:null;(r&&!r.disabled||!r&&!n)&&(this[Fe]({currentIndex:i}),e.stopPropagation())}}}}const Qs=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"menuitem",role:e.role||"menu"})}get itemRole(){return this[Be].itemRole}set itemRole(e){this[Fe]({itemRole:e})}[ke](e){super[ke]&&super[ke](e);const t=this[Be].items;if((e.items||e.itemRole)&&t){const{itemRole:e}=this[Be];t.forEach((t=>{e===Ss[t.localName]?t.removeAttribute("role"):t.setAttribute("role",e)}))}if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(Ls(Ms(ct(Fs(js(Rs(Ws(os(as(zs($s(ls(Zs(Js(V))))))))))))))),en=class extends Qs{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,highlightCurrentItem:!0,orientation:"vertical",currentItemFocused:!1})}async flashCurrentItem(){const e=this[Be].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Fe]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Fe]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[ke](e){super[ke](e),this[ie]&&(this.addEventListener("disabledchange",(e=>{this[Se]=!0;const t=e.target,{items:s}=this[Be],n=null===s?-1:s.indexOf(t);if(n>=0){const e=this[Be].availableItemFlags.slice();e[n]=!t.disabled,this[Fe]({availableItemFlags:e})}this[Se]=!1})),"PointerEvent"in window?this.addEventListener("pointerdown",(e=>this[Xe](e))):this.addEventListener("touchstart",(e=>this[Xe](e))),this.removeAttribute("tabindex"));const{currentIndex:t,items:s}=this[Be];if((e.items||e.currentIndex||e.highlightCurrentItem)&&s){const{highlightCurrentItem:e}=this[Be];s.forEach(((s,n)=>{s.toggleAttribute("current",e&&n===t)}))}(e.items||e.currentIndex||e.currentItemFocused||e.focusVisible)&&s&&s.forEach(((e,s)=>{const n=s===t,i=t<0&&0===s;this[Be].currentItemFocused?n||i||e.removeAttribute("tabindex"):(n||i)&&(e.tabIndex=0)}))}[Ae](e){if(super[Ae](e),!this[ie]&&e.currentIndex&&!this[Be].currentItemFocused){const{currentItem:e}=this[Be];(e instanceof HTMLElement?e:this).focus(),this[Fe]({currentItemFocused:!0})}}get[Me](){return this[we].content}[Ne](e,t){const s=super[Ne](e,t);if(t.currentIndex&&Object.assign(s,{currentItemFocused:!1}),t.items){const{items:t}=e,n=null===t?null:t.map((e=>!e.disabled));Object.assign(s,{availableItemFlags:n})}return s}get[Ze](){return D.html`
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
    `}},tn=class extends en{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}};customElements.define("elix-menu",class extends tn{});const sn=Symbol("documentMouseupListener");function nn(e){return class extends e{connectedCallback(){super.connectedCallback(),on(this)}get[ee](){return Object.assign(super[ee]||{},{dragSelect:!0})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),on(this)}[Ae](e){super[Ae](e),e.opened&&on(this)}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&e.opened&&Object.assign(s,{dragSelect:!0}),s}}}async function rn(e){const t=this,s=t[je].elementsFromPoint(e.clientX,e.clientY);if(t.opened){const e=s.indexOf(t[we].source)>=0,n=t[we].popup,i=s.indexOf(n)>=0,r=n.frame&&s.indexOf(n.frame)>=0;e?t[Be].dragSelect&&(t[Se]=!0,t[Fe]({dragSelect:!1}),t[Se]=!1):i||r||(t[Se]=!0,await t.close(),t[Se]=!1)}}function on(e){e[Be].opened&&e.isConnected?e[sn]||(e[sn]=rn.bind(e),document.addEventListener("mouseup",e[sn])):e[sn]&&(document.removeEventListener("mouseup",e[sn]),e[sn]=null)}function an(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{disabled:!1})}get disabled(){return this[Be].disabled}set disabled(e){this[Fe]({disabled:e})}[Ae](e){if(super[Ae]&&super[Ae](e),e.disabled&&(this.toggleAttribute("disabled",this.disabled),this[Se])){const e=new CustomEvent("disabled-changed",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("disabledchange",{bubbles:!0});this.dispatchEvent(t)}}}}const ln=Symbol("closePromise"),cn=Symbol("closeResolve");function un(e){return class extends e{attributeChangedCallback(e,t,s){if("opened"===e){const t=w(e,s);this.opened!==t&&(this.opened=t)}else super.attributeChangedCallback(e,t,s)}async close(e){super.close&&await super.close(),this[Fe]({closeResult:e}),await this.toggle(!1)}get closed(){return this[Be]&&!this[Be].opened}get closeFinished(){return this[Be].closeFinished}get closeResult(){return this[Be].closeResult}get[ee](){const e={closeResult:null,opened:!1};return this[He]&&Object.assign(e,{closeFinished:!0,effect:"close",effectPhase:"after",openCloseEffects:!0}),Object.assign(super[ee]||{},e)}async open(){super.open&&await super.open(),this[Fe]({closeResult:void 0}),await this.toggle(!0)}get opened(){return this[Be]&&this[Be].opened}set opened(e){this[Fe]({closeResult:void 0}),this.toggle(e)}[ke](e){if(super[ke](e),e.opened){const{opened:e}=this[Be];S(this,"opened",e)}if(e.closeFinished){const{closeFinished:e}=this[Be];S(this,"closed",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.opened&&this[Se]){const e=new CustomEvent("opened-changed",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});this.dispatchEvent(e);const t=new CustomEvent("openedchange",{bubbles:!0,detail:{closeResult:this[Be].closeResult,opened:this[Be].opened}});if(this.dispatchEvent(t),this[Be].opened){const e=new CustomEvent("opened",{bubbles:!0});this.dispatchEvent(e);const t=new CustomEvent("open",{bubbles:!0});this.dispatchEvent(t)}else{const e=new CustomEvent("closed",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(e);const t=new CustomEvent("close",{bubbles:!0,detail:{closeResult:this[Be].closeResult}});this.dispatchEvent(t)}}const t=this[cn];this.closeFinished&&t&&(this[cn]=null,this[ln]=null,t(this[Be].closeResult))}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.openCloseEffects||t.effect||t.effectPhase||t.opened){const{effect:t,effectPhase:n,openCloseEffects:i,opened:r}=e,o=i?"close"===t&&"after"===n:!r;Object.assign(s,{closeFinished:o})}return s}async toggle(e=!this.opened){if(super.toggle&&await super.toggle(e),e!==this[Be].opened){const t={opened:e};this[Be].openCloseEffects&&(t.effect=e?"open":"close","after"===this[Be].effectPhase&&(t.effectPhase="before")),await this[Fe](t)}}whenClosed(){return this[ln]||(this[ln]=new Promise((e=>{this[cn]=e}))),this[ln]}}}function dn(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:null})}[ke](e){if(super[ke]&&super[ke](e),e.role){const{role:e}=this[Be];e?this.setAttribute("role",e):this.removeAttribute("role")}}get role(){return super.role}set role(e){const t=String(e);super.role=t,this[De]||this[Fe]({s:t})}}}const hn=dn(V),pn=class extends hn{get[ee](){return Object.assign(super[ee],{role:"none"})}get[Ze](){return D.html`
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
    `}},mn=class extends V{get[Ze](){return D.html`
      <style>
        :host {
          display: inline-block;
          position: relative;
        }
      </style>
      <slot></slot>
    `}},gn=Symbol("appendedToDocument"),fn=Symbol("assignedZIndex"),bn=Symbol("restoreFocusToElement");function yn(e){const t=function(){const e=document.body.querySelectorAll("*"),t=Array.from(e,(e=>{const t=getComputedStyle(e);let s=0;if("static"!==t.position&&"auto"!==t.zIndex){const e=t.zIndex?parseInt(t.zIndex):0;s=isNaN(e)?0:e}return s}));return Math.max(...t)}()+1;e[fn]=t,e.style.zIndex=t.toString()}function wn(e){const t=getComputedStyle(e).zIndex,s=e.style.zIndex,n=!isNaN(parseInt(s));if("auto"===t)return n;if("0"===t&&!n){const t=e.assignedSlot||(e instanceof ShadowRoot?e.host:e.parentNode);if(!(t instanceof HTMLElement))return!0;if(!wn(t))return!1}return!0}const vn=un(function(e){return class extends e{get autoFocus(){return this[Be].autoFocus}set autoFocus(e){this[Fe]({autoFocus:e})}get[ee](){return Object.assign(super[ee]||{},{autoFocus:!0,persistent:!1})}async open(){this[Be].persistent||this.isConnected||(this[gn]=!0,document.body.append(this)),super.open&&await super.open()}[ke](e){super[ke]&&super[ke](e),this[ie]&&this.addEventListener("blur",(e=>{const t=e.relatedTarget||document.activeElement;t instanceof HTMLElement&&(x(this,t)||(this.opened?this[bn]=t:(t.focus(),this[bn]=null)))})),(e.effectPhase||e.opened||e.persistent)&&!this[Be].persistent&&((void 0===this.closeFinished?this.closed:this.closeFinished)?this[fn]&&(this.style.zIndex="",this[fn]=null):this[fn]?this.style.zIndex=this[fn]:wn(this)||yn(this))}[Ae](e){if(super[Ae]&&super[Ae](e),this[ie]&&this[Be].persistent&&!wn(this)&&yn(this),e.opened&&this[Be].autoFocus)if(this[Be].opened){this[bn]||document.activeElement===document.body||(this[bn]=document.activeElement);const e=T(this);e&&e.focus()}else this[bn]&&(this[bn].focus(),this[bn]=null);!this[ie]&&!this[Be].persistent&&this.closeFinished&&this[gn]&&(this[gn]=!1,this.parentNode&&this.parentNode.removeChild(this))}get[Ze](){const e=super[Ze]||D.html``;return e.content.append(A`
        <style>
          :host([closed]) {
            display: none;
          }
        </style>
      `),e}}}(Xs(V)));function xn(e,t,s){if(!s||s.backdropPartType){const{backdropPartType:s}=t,n=e.getElementById("backdrop");n&&K(n,s)}if(!s||s.framePartType){const{framePartType:s}=t,n=e.getElementById("frame");n&&K(n,s)}}const Tn=class extends vn{get backdrop(){return this[we]&&this[we].backdrop}get backdropPartType(){return this[Be].backdropPartType}set backdropPartType(e){this[Fe]({backdropPartType:e})}get[ee](){return Object.assign(super[ee],{backdropPartType:pn,framePartType:mn})}get frame(){return this[we].frame}get framePartType(){return this[Be].framePartType}set framePartType(e){this[Fe]({framePartType:e})}[ke](e){super[ke](e),xn(this[je],this[Be],e)}[Ae](e){super[Ae](e),e.opened&&this[Be].content&&this[Be].content.forEach((e=>{e[Z]&&e[Z]()}))}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),xn(e.content,this[Be]),e}},En=Symbol("implicitCloseListener");async function Pn(e){const t=this,s=e.relatedTarget||document.activeElement;s instanceof Element&&!x(t,s)&&(t[Se]=!0,await t.close({canceled:"window blur"}),t[Se]=!1)}async function In(e){const t=this,s="resize"!==e.type||t[Be].closeOnWindowResize;!I(t,e)&&s&&(t[Se]=!0,await t.close({canceled:"window "+e.type}),t[Se]=!1)}const Cn=as(function(e){return class extends e{constructor(){super(),this.addEventListener("blur",Pn.bind(this))}get closeOnWindowResize(){return this[Be].closeOnWindowResize}set closeOnWindowResize(e){this[Fe]({closeOnWindowResize:e})}get[ee](){return Object.assign(super[ee]||{},{closeOnWindowResize:!0,role:"alert"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super.keydown&&super.keydown(e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}[Ae](e){var t;super[Ae]&&super[Ae](e),e.opened&&(this.opened?("requestIdleCallback"in window?window.requestIdleCallback:setTimeout)((()=>{var e;this.opened&&((e=this)[En]=In.bind(e),window.addEventListener("blur",e[En]),window.addEventListener("resize",e[En]),window.addEventListener("scroll",e[En]))})):(t=this)[En]&&(window.removeEventListener("blur",t[En]),window.removeEventListener("resize",t[En]),window.removeEventListener("scroll",t[En]),t[En]=null))}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(Tn));async function Sn(e){const t=this;t[Se]=!0,await t.close({canceled:"mousedown outside"}),t[Se]=!1,e.preventDefault(),e.stopPropagation()}const kn=class extends Cn{[ke](e){super[ke](e),e.backdropPartType&&(this[we].backdrop.addEventListener("mousedown",Sn.bind(this)),"PointerEvent"in window||this[we].backdrop.addEventListener("touchend",Sn))}},Ln=Symbol("resizeListener"),On=an(at(ls(un(V))));function An(e){const t=window.innerHeight,s=window.innerWidth,n=e[we].popup.getBoundingClientRect(),i=e.getBoundingClientRect(),r=n.height,o=n.width,{horizontalAlign:a,popupPosition:l,rightToLeft:c}=e[Be],u=i.top,d=Math.ceil(t-i.bottom),h=i.right,p=Math.ceil(s-i.left),m=r<=u,g=r<=d,f="below"===l,b=f&&(g||d>=u)||!f&&!m&&d>=u,y=b&&g||!b&&m?null:b?d:u,w=b?"below":"above";let v,x,T;if("stretch"===a)v=0,x=0,T=null;else{const e="left"===a||(c?"end"===a:"start"===a),t=e&&(o<=p||p>=h)||!e&&!(o<=h)&&p>=h;v=t?0:null,x=t?null:0,T=t&&p||!t&&h?null:t?p:h}e[Fe]({calculatedFrameMaxHeight:y,calculatedFrameMaxWidth:T,calculatedPopupLeft:v,calculatedPopupPosition:w,calculatedPopupRight:x,popupMeasured:!0})}function Dn(e,t,s){if(!s||s.popupPartType){const{popupPartType:s}=t,n=e.getElementById("popup");n&&K(n,s)}if(!s||s.sourcePartType){const{sourcePartType:s}=t,n=e.getElementById("source");n&&K(n,s)}}const Mn=class extends On{get[ee](){return Object.assign(super[ee],{ariaHasPopup:"true",horizontalAlign:"start",popupHeight:null,popupMeasured:!1,popupPosition:"below",popupPartType:kn,popupWidth:null,roomAbove:null,roomBelow:null,roomLeft:null,roomRight:null,sourcePartType:"div"})}get[ve](){return this[we].source}get frame(){return this[we].popup.frame}get horizontalAlign(){return this[Be].horizontalAlign}set horizontalAlign(e){this[Fe]({horizontalAlign:e})}get popupPosition(){return this[Be].popupPosition}set popupPosition(e){this[Fe]({popupPosition:e})}get popupPartType(){return this[Be].popupPartType}set popupPartType(e){this[Fe]({popupPartType:e})}[ke](e){if(super[ke](e),Dn(this[je],this[Be],e),this[ie]||e.ariaHasPopup){const{ariaHasPopup:e}=this[Be];null===e?this[ve].removeAttribute("aria-haspopup"):this[ve].setAttribute("aria-haspopup",this[Be].ariaHasPopup)}if(e.popupPartType&&(this[we].popup.addEventListener("open",(()=>{this.opened||(this[Se]=!0,this.open(),this[Se]=!1)})),this[we].popup.addEventListener("close",(e=>{if(!this.closed){this[Se]=!0;const t=e.detail.closeResult;this.close(t),this[Se]=!1}}))),e.opened||e.popupMeasured){const{calculatedFrameMaxHeight:e,calculatedFrameMaxWidth:t,calculatedPopupLeft:s,calculatedPopupPosition:n,calculatedPopupRight:i,opened:r,popupMeasured:o}=this[Be];if(r)if(o){const r="below"===n,o=this[we].popup;Object.assign(o.style,{bottom:r?"":0,left:s,opacity:"",right:i});const a=o.frame;Object.assign(a.style,{maxHeight:e?e+"px":"",maxWidth:t?t+"px":""}),Object.assign(this[we].popupContainer.style,{overflow:"",top:r?"":"0"})}else Object.assign(this[we].popupContainer.style,{overflow:"hidden"}),Object.assign(this[we].popup.style,{opacity:0});else r||(Object.assign(this[we].popupContainer.style,{overflow:""}),Object.assign(this[we].popup.style,{bottom:"",left:"",opacity:"",position:"",right:""}))}if(e.opened){const{opened:e}=this[Be];this[we].popup.opened=e}if(e.disabled&&"disabled"in this[we].source){const{disabled:e}=this[Be];this[we].source.disabled=e}if(e.calculatedPopupPosition){const{calculatedPopupPosition:e}=this[Be],t=this[we].popup;"position"in t&&(t.position=e)}}[Ae](e){super[Ae](e);const{opened:t}=this[Be];var s;e.opened?t?(s=this,setTimeout((()=>{s[Be].opened&&(An(s),function(e){const t=e;t[Ln]=()=>{An(e)},window.addEventListener("resize",t[Ln])}(s))}))):function(e){const t=e;t[Ln]&&(window.removeEventListener("resize",t[Ln]),t[Ln]=null)}(this):t&&!this[Be].popupMeasured&&An(this)}get sourcePartType(){return this[Be].sourcePartType}set sourcePartType(e){this[Fe]({sourcePartType:e})}[Ne](e,t){const s=super[Ne](e,t);return(t.opened&&!e.opened||e.opened&&(t.horizontalAlign||t.rightToLeft))&&Object.assign(s,{calculatedFrameMaxHeight:null,calculatedFrameMaxWidth:null,calculatedPopupLeft:null,calculatedPopupPosition:null,calculatedPopupRight:null,popupMeasured:!1}),s}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),Dn(e.content,this[Be]),e}},Fn=ct(as(nn(Mn))),jn=class extends Fn{get[ee](){return Object.assign(super[ee],{sourcePartType:"button"})}[Te](e){let t;switch(e.key){case" ":case"ArrowDown":case"ArrowUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0);break;case"Escape":this.opened&&(this.close({canceled:"Escape"}),t=!0)}if(t=super[Te]&&super[Te](e),!t&&this.opened&&!e.metaKey&&!e.altKey)switch(e.key){case"ArrowDown":case"ArrowLeft":case"ArrowRight":case"ArrowUp":case"End":case"Home":case"PageDown":case"PageUp":case" ":t=!0}return t}[ke](e){if(super[ke](e),this[ie]&&this[we].source.addEventListener("focus",(async e=>{const t=I(this[we].popup,e),s=null!==this[Be].popupHeight;!t&&this.opened&&s&&(this[Se]=!0,await this.close(),this[Se]=!1)})),e.opened){const{opened:e}=this[Be];this.toggleAttribute("opened",e)}e.sourcePartType&&this[we].source.addEventListener("mousedown",(e=>{if(this.disabled)return void e.preventDefault();const t=e;t.button&&0!==t.button||(setTimeout((()=>{this.opened||(this[Se]=!0,this.open(),this[Se]=!1)})),e.stopPropagation())})),e.popupPartType&&this[we].popup.removeAttribute("tabindex")}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}},Rn=Symbol("documentMousemoveListener");function Hn(e){return class extends e{connectedCallback(){super.connectedCallback(),Nn(this)}get[ee](){return Object.assign(super[ee]||{},{currentIndex:-1,hasHoveredOverItemSinceOpened:!1,popupList:null})}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Nn(this)}[Te](e){let t=!1;switch(e.key){case"Enter":this.opened&&(Wn(this),t=!0)}return t||super[Te]&&super[Te](e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.popupList){const{popupList:e}=this[Be];e&&(e.addEventListener("mouseup",(async e=>{const t=this[Be].currentIndex;this[Be].dragSelect||t>=0?(e.stopPropagation(),this[Se]=!0,await Wn(this),this[Se]=!1):e.stopPropagation()})),e.addEventListener("currentindexchange",(e=>{this[Se]=!0;const t=e;this[Fe]({currentIndex:t.detail.currentIndex}),this[Se]=!1})))}if(e.currentIndex||e.popupList){const{currentIndex:e,popupList:t}=this[Be];t&&"currentIndex"in t&&(t.currentIndex=e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.opened){if(this[Be].opened){const{popupList:e}=this[Be];e.scrollCurrentItemIntoView&&setTimeout((()=>{e.scrollCurrentItemIntoView()}))}Nn(this)}}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};return t.opened&&e.opened&&Object.assign(s,{hasHoveredOverItemSinceOpened:!1}),s}}}function Bn(e){const t=this,{hasHoveredOverItemSinceOpened:s,opened:n}=t[Be];if(n){const n=e.composedPath?e.composedPath()[0]:e.target,i=t.items;if(n&&n instanceof Node&&i){const e=P(i,n),r=i[e],o=r&&!r.disabled?e:-1;(s||o>=0)&&o!==t[Be].currentIndex&&(t[Se]=!0,t[Fe]({currentIndex:o}),o>=0&&!s&&t[Fe]({hasHoveredOverItemSinceOpened:!0}),t[Se]=!1)}}}function Nn(e){e[Be].opened&&e.isConnected?e[Rn]||(e[Rn]=Bn.bind(e),document.addEventListener("mousemove",e[Rn])):e[Rn]&&(document.removeEventListener("mousemove",e[Rn]),e[Rn]=null)}async function Wn(e){const t=e[Se],s=e[Be].currentIndex>=0,n=e.items;if(n){const i=s?n[e[Be].currentIndex]:void 0,r=e[Be].popupList;s&&"flashCurrentItem"in r&&await r.flashCurrentItem();const o=e[Se];e[Se]=t,await e.close(i),e[Se]=o}}const zn=Hn(jn);function Yn(e,t,s){if(!s||s.menuPartType){const{menuPartType:s}=t,n=e.getElementById("menu");n&&K(n,s)}}const Un=class extends zn{get[ee](){return Object.assign(super[ee],{menuPartType:en})}get items(){const e=this[we]&&this[we].menu;return e?e.items:null}get menuPartType(){return this[Be].menuPartType}set menuPartType(e){this[Fe]({menuPartType:e})}[ke](e){if(super[ke](e),Yn(this[je],this[Be],e),e.menuPartType&&(this[we].menu.addEventListener("blur",(async e=>{const t=e.relatedTarget||document.activeElement;this.opened&&!x(this[we].menu,t)&&(this[Se]=!0,await this.close(),this[Se]=!1)})),this[we].menu.addEventListener("mousedown",(e=>{0===e.button&&this.opened&&(e.stopPropagation(),e.preventDefault())}))),e.opened){const{opened:e}=this[Be];this[we].source.setAttribute("aria-expanded",e.toString())}}[Ae](e){super[Ae](e),e.menuPartType&&this[Fe]({popupList:this[we].menu})}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&!e.opened&&Object.assign(s,{currentIndex:-1}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
        <div id="menu" part="menu">
          <slot></slot>
        </div>
      `),Yn(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="menu"] {
          max-height: 100%;
        }
      </style>
    `),e}},qn=an(V),Vn=class extends qn{get[ee](){return Object.assign(super[ee],{direction:"down"})}get direction(){return this[Be].direction}set direction(e){this[Fe]({direction:e})}[ke](e){if(super[ke](e),e.direction){const{direction:e}=this[Be];this[we].downIcon.style.display="down"===e?"block":"none",this[we].upIcon.style.display="up"===e?"block":"none"}}get[Ze](){return D.html`
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
    `}};function $n(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{popupTogglePartType:Vn})}get popupTogglePartType(){return this[Be].popupTogglePartType}set popupTogglePartType(e){this[Fe]({popupTogglePartType:e})}[ke](e){if(super[ke](e),_n(this[je],this[Be],e),e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[Be],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.disabled){const{disabled:e}=this[Be];this[we].popupToggle.disabled=e}}get[Ze](){const e=super[Ze],t=e.content.querySelector('[part~="source"]');return t&&t.append(A`
          <div
            id="popupToggle"
            part="popup-toggle"
            exportparts="toggle-icon, down-icon, up-icon"
            tabindex="-1"
          ></div>
      `),_n(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="popup-toggle"] {
          outline: none;
        }

        [part~="source"] {
          align-items: center;
          display: flex;
        }
      </style>
    `),e}}}function _n(e,t,s){if(!s||s.popupTogglePartType){const{popupTogglePartType:s}=t,n=e.getElementById("popupToggle");n&&K(n,s)}}const Gn=class extends fs{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          [part~="button"] {
            background: #eee;
            border: 1px solid #ccc;
            padding: 0.25em 0.5em;
          }
        </style>
      `),e}},Kn=class extends Vn{get[Ze](){const e=super[Ze],t=e.content.getElementById("downIcon"),s=A`
      <svg
        id="downIcon"
        part="toggle-icon down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 0 l5 5 5 -5 z" />
      </svg>
    `.firstElementChild;t&&s&&G(t,s);const n=e.content.getElementById("upIcon"),i=A`
      <svg
        id="upIcon"
        part="toggle-icon up-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 10 5"
      >
        <path d="M 0 5 l5 -5 5 5 z" />
      </svg>
    `.firstElementChild;return n&&i&&G(n,i),e.content.append(A`
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
      `),e}},Xn=class extends pn{},Zn=class extends mn{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            background: white;
            border: 1px solid rgba(0, 0, 0, 0.2);
            box-shadow: 0 0px 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
          }
        </style>
      `),e}},Jn=class extends kn{get[ee](){return Object.assign(super[ee],{backdropPartType:Xn,framePartType:Zn})}};class Qn extends($n(Un)){get[ee](){return Object.assign(super[ee],{menuPartType:tn,popupPartType:Jn,popupTogglePartType:Kn,sourcePartType:Gn})}get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          [part~="menu"] {
            background: window;
            border: none;
            padding: 0.5em 0;
          }
        </style>
      `),e}}const ei=Qn;function ti(e){return class extends e{constructor(){super();!this[Ce]&&this.attachInternals&&(this[Ce]=this.attachInternals())}attributeChangedCallback(e,t,s){if("current"===e){const t=w(e,s);this.current!==t&&(this.current=t)}else super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee]||{},{current:!1})}[ke](e){if(super[ke](e),e.current){const{current:e}=this[Be];S(this,"current",e)}}[Ae](e){if(super[Ae]&&super[Ae](e),e.current){const{current:e}=this[Be],t=new CustomEvent("current-changed",{bubbles:!0,detail:{current:e}});this.dispatchEvent(t);const s=new CustomEvent("currentchange",{bubbles:!0,detail:{current:e}});this.dispatchEvent(s)}}get current(){return this[Be].current}set current(e){this[Fe]({current:e})}}}customElements.define("elix-menu-button",class extends ei{});class si extends(ti(an($t(V)))){}const ni=si,ii=class extends ni{get[Ze](){return D.html`
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
    `}};customElements.define("elix-menu-item",class extends ii{});const ri=class extends V{get disabled(){return!0}[ke](e){super[ke](e),this[ie]&&this.setAttribute("aria-hidden","true")}},oi=class extends ri{get[Ze](){return D.html`
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
    `}};customElements.define("elix-menu-separator",class extends oi{});class ai extends($n(jn)){get[ee](){return Object.assign(super[ee],{popupPartType:Jn,sourcePartType:Gn})}}const li=ai;customElements.define("elix-popup-button",class extends li{}),customElements.define("elix-popup",class extends Jn{});const ci=Symbol("previousBodyStyleOverflow"),ui=Symbol("previousDocumentMarginRight");function di(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{role:"dialog"})}[Te](e){let t=!1;switch(e.key){case"Escape":this.close({canceled:"Escape"}),t=!0}return t||super[Te]&&super[Te](e)||!1}[ke](e){if(super[ke]&&super[ke](e),e.opened)if(this[Be].opened&&document.documentElement){const e=document.documentElement.clientWidth,t=window.innerWidth-e;this[ci]=document.body.style.overflow,this[ui]=t>0?document.documentElement.style.marginRight:null,document.body.style.overflow="hidden",t>0&&(document.documentElement.style.marginRight=t+"px")}else null!=this[ci]&&(document.body.style.overflow=this[ci],this[ci]=null),null!=this[ui]&&(document.documentElement.style.marginRight=this[ui],this[ui]=null);if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}const hi=Symbol("wrap"),pi=Symbol("wrappingFocus");function mi(e){return class extends e{[Te](e){const t=T(this[je]);if(t){const s=document.activeElement&&(document.activeElement===t||document.activeElement.contains(t)),n=this[je].activeElement,i=n&&(n===t||x(n,t));(s||i)&&"Tab"===e.key&&e.shiftKey&&(this[pi]=!0,this[we].focusCatcher.focus(),this[pi]=!1)}return super[Te]&&super[Te](e)||!1}[ke](e){super[ke]&&super[ke](e),this[ie]&&this[we].focusCatcher.addEventListener("focus",(()=>{if(!this[pi]){const e=T(this[je]);e&&e.focus()}}))}[hi](e){const t=A`
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
      `,s=t.getElementById("focusCaptureContainer");s&&(e.replaceWith(t),s.append(e))}}}mi.wrap=hi;const gi=mi,fi=class extends pn{constructor(){super(),"PointerEvent"in window||this.addEventListener("touchmove",(e=>{1===e.touches.length&&e.preventDefault()}))}},bi=di(gi(as(Tn))),yi=class extends bi{get[ee](){return Object.assign(super[ee],{backdropPartType:fi,tabIndex:-1})}get[Ze](){const e=super[Ze],t=e.content.querySelector("#frame");return this[gi.wrap](t),e.content.append(A`
        <style>
          :host {
            height: 100%;
            left: 0;
            pointer-events: initial;
            top: 0;
            width: 100%;
          }
        </style>
      `),e}},wi=class extends fi{get[Ze](){const e=super[Ze];return e.content.append(A`
        <style>
          :host {
            background: rgba(0, 0, 0, 0.2);
          }
        </style>
      `),e}};function vi(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{backdropPartType:wi,framePartType:Zn})}}}class xi extends(vi(yi)){}const Ti=xi;function Ei(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectedText:""})}[oe](e){return super[oe]?super[oe](e):Ns(e)}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?this[oe](i):"";Object.assign(s,{selectedText:r})}return s}get selectedText(){return this[Be].selectedText}set selectedText(e){const{items:t}=this[Be],s=t?function(e,t,s){return e.findIndex((e=>t(e)===s))}(t,this[oe],String(e)):-1;this[Fe]({selectedIndex:s})}}}function Pi(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{value:""})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.items||t.selectedIndex){const{items:t,selectedIndex:n}=e,i=t?t[n]:null,r=i?i.getAttribute("value"):"";Object.assign(s,{value:r})}return s}get value(){return this[Be].value}set value(e){const{items:t}=this[Be],s=t?function(e,t){return e.findIndex((e=>e.getAttribute("value")===t))}(t,String(e)):-1;this[Fe]({selectedIndex:s})}}}function Ii(e){return class extends e{attributeChangedCallback(e,t,s){"selected-index"===e?this.selectedIndex=Number(s):super.attributeChangedCallback(e,t,s)}[Ae](e){if(super[Ae]&&super[Ae](e),e.selectedIndex&&this[Se]){const e=this[Be].selectedIndex,t=new CustomEvent("selected-index-changed",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(t);const s=new CustomEvent("selectedindexchange",{bubbles:!0,detail:{selectedIndex:e}});this.dispatchEvent(s)}}get selectedIndex(){const{items:e,selectedIndex:t}=this[Be];return e&&e.length>0?t:-1}set selectedIndex(e){isNaN(e)||this[Fe]({selectedIndex:e})}get selectedItem(){const{items:e,selectedIndex:t}=this[Be];return e&&e[t]}set selectedItem(e){const{items:t}=this[Be];if(!t)return;const s=t.indexOf(e);s>=0&&this[Fe]({selectedIndex:s})}}}customElements.define("elix-dialog",class extends Ti{});const Ci=Ls(st(rs(js(Rs(Hn(Ei(Pi(Ii(Zs(jn))))))))));function Si(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&K(n,s)}if(!s||s.valuePartType){const{valuePartType:s}=t,n=e.getElementById("value");n&&K(n,s)}}const ki=class extends Ci{[X](e,t){L(t,(e?[...e.childNodes]:[]).map((e=>e.cloneNode(!0))))}get[ee](){return Object.assign(super[ee],{ariaHasPopup:"listbox",listPartType:en,selectedIndex:-1,selectedItem:null,valuePartType:"div"})}get items(){const e=this[we]&&this[we].list;return e?e.items:null}get listPartType(){return this[Be].listPartType}set listPartType(e){this[Fe]({listPartType:e})}[ke](e){if(super[ke](e),Si(this[je],this[Be],e),e.items||e.selectedIndex){const{items:e,selectedIndex:t}=this[Be],s=e?e[t]:null;this[X](s,this[we].value),e&&e.forEach((e=>{"selected"in e&&(e.selected=e===s)}))}if(e.opened){const{opened:e}=this[Be];this[we].source.setAttribute("aria-expanded",e.toString())}if(e.sourcePartType){const e=this[we].source;e.inner&&e.inner.setAttribute("role","none")}}[Ae](e){super[Ae](e),e.listPartType&&this[Fe]({popupList:this[we].list})}[Ne](e,t){const s=super[Ne](e,t);if(t.opened&&e.opened&&Object.assign(s,{currentIndex:e.selectedIndex}),t.opened){const{closeResult:n,currentIndex:i,opened:r}=e,o=t.opened&&!r,a=n&&n.canceled;o&&!a&&i>=0&&Object.assign(s,{selectedIndex:i})}if(t.items||t.selectedIndex){const{items:t,opened:n,selectedIndex:i}=e;!n&&i<0&&t&&t.length>0&&Object.assign(s,{selectedIndex:0})}return s}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="source"]');t&&G(t,A` <div id="value" part="value"></div> `);const s=e.content.querySelector("slot:not([name])");s&&s.replaceWith(A`
        <div id="list" part="list">
          <slot></slot>
        </div>
      `);const n=e.content.querySelector('[part~="source"]');return n&&(n.setAttribute("aria-activedescendant","value"),n.setAttribute("aria-autocomplete","none"),n.setAttribute("aria-controls","list"),n.setAttribute("role","combobox")),Si(e.content,this[Be]),e.content.append(A`
      <style>
        [part~="list"] {
          max-height: 100%;
        }
      </style>
    `),e}get valuePartType(){return this[Be].valuePartType}set valuePartType(e){this[Fe]({valuePartType:e})}},Li=dn(tt(Ls(Ms(Fs(at(rs(js(Rs(Ws(os(as(zs($s(ls(Ii(Ei(Pi(Zs(Js(V)))))))))))))))))))),Oi=class extends Li{get[ee](){return Object.assign(super[ee],{highlightCurrentItem:!0,orientation:"vertical",role:"listbox"})}async flashCurrentItem(){const e=this[Be].focusVisible,t=matchMedia("(pointer: fine)").matches;if(e||t){const e=75;this[Fe]({highlightCurrentItem:!1}),await new Promise((t=>setTimeout(t,e))),this[Fe]({highlightCurrentItem:!0}),await new Promise((t=>setTimeout(t,e)))}}[ke](e){if(super[ke](e),e.items||e.currentIndex||e.highlightCurrentItem){const{currentIndex:e,items:t,highlightCurrentItem:s}=this[Be];t&&t.forEach(((t,n)=>{const i=n===e;t.toggleAttribute("current",s&&i),t.setAttribute("aria-selected",String(i))}))}}get[Me](){return this[we].container}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),e}},Ai=class extends Oi{get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),e}};class Di extends($n(ki)){get[ee](){return Object.assign(super[ee],{listPartType:Ai,popupPartType:Jn,sourcePartType:Gn,popupTogglePartType:Kn})}}const Mi=Di;customElements.define("elix-dropdown-list",class extends Mi{});class Fi extends(dn(ti(an($t(V))))){get[ee](){return Object.assign(super[ee],{role:"option"})}get[Ze](){return D.html`
      <style>
        :host {
          display: block;
        }
      </style>
      <slot></slot>
    `}}const ji=Fi,Ri=class extends ji{get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
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

    `),e}};customElements.define("elix-option",class extends Ri{});const Hi=Symbol("deferToScrolling"),Bi=Symbol("multiTouch"),Ni=Symbol("previousTime"),Wi=Symbol("previousVelocity"),zi=Symbol("previousX"),Yi=Symbol("previousY"),Ui=Symbol("startX"),qi=Symbol("startY"),Vi=Symbol("touchSequenceAxis");function $i(e){return"pen"===e.pointerType||"touch"===e.pointerType&&e.isPrimary}function _i(e,t,s,n){const i=e,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[Be],l=t-i[zi],c=s-i[Yi],u=Date.now(),d="vertical"===r?c:l,h=d/(u-i[Ni])*1e3;i[zi]=t,i[Yi]=s,i[Ni]=u,i[Wi]=h;const p=Math.abs(c)>Math.abs(l)?"vertical":"horizontal";if(null===i[Vi])i[Vi]=p;else if(p!==i[Vi])return!0;if(p!==r)return!1;if(i[Hi]&&Os(n,r,d<0))return!1;i[Ui]||(i[Ui]=t),i[qi]||(i[qi]=s);const m=function(e,t,s){const{swipeAxis:n}=e[Be],i=e,r="vertical"===n,o=r?s-i[qi]:t-i[Ui],a=r?e[Ke].offsetHeight:e[Ke].offsetWidth;return a>0?o/a:0}(e,t,s),g=Math.max(Math.min(m,o),a);return e[Be].swipeFraction!==g&&(i[Hi]=!1,e[Fe]({swipeFraction:g}),!0)}function Gi(e,t,s,n){const i=e[Wi],{swipeAxis:r,swipeFraction:o}=e[Be],a="vertical"===r;let l=!1;if(e[Hi]&&(l=Os(n,r,i<0)),!l){let t;if(i>=800&&o>=0?(t=!0,a?e[Fe]({swipeDownWillCommit:!0}):e[Fe]({swipeRightWillCommit:!0})):i<=-800&&o<=0?(t=!1,a?e[Fe]({swipeUpWillCommit:!0}):e[Fe]({swipeLeftWillCommit:!0})):e[Be].swipeLeftWillCommit||e[Be].swipeUpWillCommit?t=!1:(e[Be].swipeRightWillCommit||e[Be].swipeDownWillCommit)&&(t=!0),void 0!==t){const s=a?t?We:$e:t?qe:Ye;s&&e[s]&&e[s]()}}e[Vi]=null,e[Fe]({swipeFraction:null})}function Ki(e,t,s){const n=e;n[Hi]=!0,n[Ni]=Date.now(),n[Wi]=0,n[zi]=t,n[Yi]=s,n[Ui]=null,n[qi]=null,n[Vi]=null,e[Fe]({swipeFraction:0}),e[Ge]&&e[Ge](t,s)}const Xi=Symbol("absorbDeceleration"),Zi=Symbol("deferToScrolling"),Ji=Symbol("lastDeltaX"),Qi=Symbol("lastDeltaY"),er=Symbol("lastWheelTimeout"),tr=Symbol("postGestureDelayComplete"),sr=Symbol("wheelDistance"),nr=Symbol("wheelSequenceAxis");function ir(e){const t=e;t[Xi]=!1,t[Zi]=!0,t[Ji]=0,t[Qi]=0,t[tr]=!0,t[sr]=0,t[nr]=null,t[er]&&(clearTimeout(t[er]),t[er]=null)}const rr=di(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{enableEffects:!1})}[Ae](e){super[Ae]&&super[Ae](e),this[ie]&&setTimeout((()=>{this[Fe]({enableEffects:!0})}))}}}(gi(as(ls(function(e){return class extends e{[ke](e){super[ke]&&super[ke](e),this[ie]&&("TouchEvent"in window?(this.addEventListener("touchstart",(async e=>{if(this[Se]=!0,!this[Bi]){if(1===e.touches.length){const{clientX:t,clientY:s}=e.changedTouches[0];Ki(this,t,s)}else this[Bi]=!0;await Promise.resolve(),this[Se]=!1}})),this.addEventListener("touchmove",(async e=>{if(this[Se]=!0,!this[Bi]&&1===e.touches.length&&e.target){const{clientX:t,clientY:s}=e.changedTouches[0];_i(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Se]=!1})),this.addEventListener("touchend",(async e=>{if(this[Se]=!0,0===e.touches.length&&e.target){if(!this[Bi]){const{clientX:t,clientY:s}=e.changedTouches[0];Gi(this,0,0,e.target)}this[Bi]=!1}await Promise.resolve(),this[Se]=!1}))):"PointerEvent"in window&&(this.addEventListener("pointerdown",(async e=>{if(this[Se]=!0,$i(e)){const{clientX:t,clientY:s}=e;Ki(this,t,s)}await Promise.resolve(),this[Se]=!1})),this.addEventListener("pointermove",(async e=>{if(this[Se]=!0,$i(e)&&e.target){const{clientX:t,clientY:s}=e;_i(this,t,s,e.target)&&(e.preventDefault(),e.stopPropagation())}await Promise.resolve(),this[Se]=!1})),this.addEventListener("pointerup",(async e=>{if(this[Se]=!0,$i(e)&&e.target){const{clientX:t,clientY:s}=e;Gi(this,0,0,e.target)}await Promise.resolve(),this[Se]=!1}))),this.style.touchAction="TouchEvent"in window?"manipulation":"none")}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeStartX:null,swipeStartY:null,swipeUpWillCommit:!1})}get[Ke](){return super[Ke]||this}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{constructor(){super(),this.addEventListener("wheel",(async e=>{this[Se]=!0,function(e,t){const s=e;s[er]&&clearTimeout(s[er]),s[er]=setTimeout((async()=>{e[Se]=!0,async function(e){let t;e[Be].swipeDownWillCommit?t=We:e[Be].swipeLeftWillCommit?t=Ye:e[Be].swipeRightWillCommit?t=qe:e[Be].swipeUpWillCommit&&(t=$e),ir(e),e[Fe]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1}),t&&e[t]&&await e[t]()}(e),await Promise.resolve(),s[Se]=!1}),100);const n=t.deltaX,i=t.deltaY,{swipeAxis:r,swipeFractionMax:o,swipeFractionMin:a}=e[Be],l="vertical"===r,c=l?Math.sign(i)*(i-s[Qi]):Math.sign(n)*(n-s[Ji]);s[Ji]=n,s[Qi]=i;const u=null===s[nr],d=Math.abs(i)>Math.abs(n)?"vertical":"horizontal";if(!u&&d!==s[nr])return!0;if(d!==r)return!1;if(!s[tr])return!0;if(c>0)s[Xi]=!1;else if(s[Xi])return!0;if(s[Zi]&&Os(e[Me]||e,r,(l?i:n)>0))return!1;s[Zi]=!1,u&&(s[nr]=d,e[Ge]&&e[Ge](t.clientX,t.clientY)),s[sr]-=l?i:n;const h=l?s[Ke].offsetHeight:s[Ke].offsetWidth;let p=h>0?s[sr]/h:0;p=Math.sign(p)*Math.min(Math.abs(p),1);const m=Math.max(Math.min(p,o),a);let g;return-1===m?g=l?$e:Ye:1===m&&(g=l?We:qe),g?function(e,t){e[t]&&e[t]();const s=e;s[Xi]=!0,s[Zi]=!0,s[tr]=!1,s[sr]=0,s[nr]=null,setTimeout((()=>{s[tr]=!0}),250),e[Fe]({swipeDownWillCommit:!1,swipeFraction:null,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}(e,g):e[Fe]({swipeFraction:m}),!0}(this,e)&&(e.preventDefault(),e.stopPropagation()),await Promise.resolve(),this[Se]=!1})),ir(this)}get[ee](){return Object.assign(super[ee]||{},{swipeAxis:"horizontal",swipeDownWillCommit:!1,swipeFraction:null,swipeFractionMax:1,swipeFractionMin:-1,swipeLeftWillCommit:!1,swipeRightWillCommit:!1,swipeUpWillCommit:!1})}get[Ke](){return super[Ke]||this}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.swipeFraction){const{swipeAxis:t,swipeFraction:n}=e;null!==n&&("horizontal"===t?Object.assign(s,{swipeLeftWillCommit:n<=-.5,swipeRightWillCommit:n>=.5}):Object.assign(s,{swipeUpWillCommit:n<=-.5,swipeDownWillCommit:n>=.5}))}return s}}}(function(e){return class extends e{get[ne](){return super[ne]||this}[ke](e){super[ke]&&super[ke](e),this[ie]&&(this[ne]===this?this:this[je]).addEventListener("transitionend",(e=>{const t=this[Be].effectEndTarget||this[ne];e.target===t&&this[Fe]({effectPhase:"after"})}))}[Ae](e){if(super[Ae]&&super[Ae](e),e.effect||e.effectPhase){const{effect:e,effectPhase:t}=this[Be],s=new CustomEvent("effect-phase-changed",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(s);const n=new CustomEvent("effectphasechange",{bubbles:!0,detail:{effect:e,effectPhase:t}});this.dispatchEvent(n),e&&("after"!==t&&this.offsetHeight,"before"===t&&this[Fe]({effectPhase:"during"}))}}async[He](e){await this[Fe]({effect:e,effectPhase:"before"})}}}(Tn))))))));async function or(e){e[Fe]({effect:"close",effectPhase:"during"}),await e.close()}async function ar(e){e[Fe]({effect:"open",effectPhase:"during"}),await e.open()}const lr=class extends rr{get[ee](){return Object.assign(super[ee],{backdropPartType:fi,drawerTransitionDuration:250,fromEdge:"start",gripSize:null,openedFraction:0,openedRenderedFraction:0,persistent:!0,role:"landmark",showTransition:!1,tabIndex:-1})}get[ne](){return this[we].frame}get fromEdge(){return this[Be].fromEdge}set fromEdge(e){this[Fe]({fromEdge:e})}get gripSize(){return this[Be].gripSize}set gripSize(e){this[Fe]({gripSize:e})}[ke](e){if(super[ke](e),e.backdropPartType&&this[we].backdrop.addEventListener("click",(async()=>{this[Se]=!0,await this.close(),this[Se]=!1})),e.gripSize||e.opened||e.swipeFraction){const{gripSize:e,opened:t,swipeFraction:s}=this[Be],n=null!==s,i=t||n;this.style.pointerEvents=i?"initial":"none";const r=!(null!==e||i);this[we].frame.style.clipPath=r?"inset(0px)":""}if(e.effect||e.effectPhase||e.fromEdge||e.gripSize||e.openedFraction||e.rightToLeft||e.swipeFraction){const{drawerTransitionDuration:e,effect:t,effectPhase:s,fromEdge:n,gripSize:i,openedFraction:r,openedRenderedFraction:o,rightToLeft:a,showTransition:l,swipeFraction:c}=this[Be],u="left"===n||"top"===n||"start"===n&&!a||"end"===n&&a?-1:1,d=u*(1-r);null!==c||"open"===t&&"before"===s?this[we].backdrop.style.visibility="visible":"close"===t&&"after"===s&&(this[we].backdrop.style.visibility="hidden");const h=Math.abs(r-o),p=l?h*(e/1e3):0,m=100*d+"%",g=i?i*-u*(1-r):0,f=`translate${"top"===n||"bottom"===n?"Y":"X"}(${0===g?m:`calc(${m} + ${g}px)`})`;Object.assign(this[we].frame.style,{transform:f,transition:l?`transform ${p}s`:""})}if(e.fromEdge||e.rightToLeft){const{fromEdge:e,rightToLeft:t}=this[Be],s={bottom:0,left:0,right:0,top:0},n={bottom:"top",left:"right",right:"left",top:"bottom"};n.start=n[t?"right":"left"],n.end=n[t?"left":"right"],Object.assign(this.style,s,{[n[e]]:null});const i={bottom:"flex-end",end:"flex-end",left:t?"flex-end":"flex-start",right:t?"flex-start":"flex-end",start:"flex-start",top:"flex-start"};this.style.flexDirection="top"===e||"bottom"===e?"column":"row",this.style.justifyContent=i[e]}e.opened&&this.setAttribute("aria-expanded",this[Be].opened.toString())}[Ae](e){super[Ae](e),e.opened&&S(this,"opened",this[Be].opened),e.openedFraction&&this[Fe]({openedRenderedFraction:this[Be].openedFraction})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.fromEdge){const{fromEdge:t}=e,n="top"===t||"bottom"===t?"vertical":"horizontal";Object.assign(s,{swipeAxis:n})}if(t.effect||t.effectPhase||t.fromEdge||t.rightToLeft||t.swipeFraction){const{effect:t,effectPhase:n,fromEdge:i,rightToLeft:r,swipeFraction:o}=e,a="open"===t&&"before"!==n||"close"===t&&"before"===n,l="left"===i||"top"===i||"start"===i&&!r||"end"===i&&r,c=.999,u=l&&!a||!l&&a,d=u?0:-c,h=u?c:0,p=l?-1:1;let m=a?1:0;null!==o&&(m-=p*Math.max(Math.min(o,h),d)),Object.assign(s,{openedFraction:m})}if(t.enableEffects||t.effect||t.effectPhase||t.swipeFraction){const{enableEffects:t,effect:n,effectPhase:i,swipeFraction:r}=e,o=null!==r,a=t&&!o&&n&&("during"===i||"after"===i);Object.assign(s,{showTransition:a})}return s}async[We](){const{fromEdge:e}=this[Be];"top"===e?ar(this):"bottom"===e&&or(this)}async[Ye](){const{fromEdge:e,rightToLeft:t}=this[Be],s="left"===e||"start"===e&&!t||"end"===e&&t;"right"===e||"start"===e&&t||"end"===e&&!t?ar(this):s&&or(this)}async[qe](){const{fromEdge:e,rightToLeft:t}=this[Be],s="right"===e||"start"===e&&t||"end"===e&&!t;"left"===e||"start"===e&&!t||"end"===e&&t?ar(this):s&&or(this)}async[$e](){const{fromEdge:e}=this[Be];"bottom"===e?ar(this):"top"===e&&or(this)}get[Me](){return this[we].frame}get[Ke](){return this[we].frame}get[Ze](){const e=super[Ze],t=e.content.querySelector("#frameContent");return this[gi.wrap](t),e.content.append(A`
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
      `),e}};class cr extends(function(e){return class extends e{[ke](e){if(super[ke]&&super[ke](e),e.openedFraction){const{drawerTransitionDuration:e,openedFraction:t,openedRenderedFraction:s,showTransition:n}=this[Be],i=Math.abs(t-s),r=n?i*(e/1e3):0;Object.assign(this[we].backdrop.style,{opacity:t,transition:n?`opacity ${r}s linear`:""})}}}}(vi(lr))){}const ur=cr;function dr(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectionEnd:null,selectionStart:null})}[ke](e){if(super[ke]&&super[ke](e),this[ie]){const e=(()=>{setTimeout((()=>{this[Se]=!0,function(e){const t=e.inner,{selectionEnd:s,selectionStart:n}=t;e[Fe]({selectionEnd:s,selectionStart:n})}(this),this[Se]=!1}),10)}).bind(this);this.addEventListener("keydown",e),this.addEventListener("mousedown",e)}}[Ae](e){super[Ae]&&super[Ae](e);const{selectionEnd:t,selectionStart:s}=this[Be];null===t&&this[Fe]({selectionEnd:this.inner.selectionEnd}),null===s&&this[Fe]({selectionStart:this.inner.selectionStart})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};return t.value&&Object.assign(s,{selectionStart:null,selectionEnd:null}),s}}}customElements.define("elix-drawer",class extends ur{});const hr=st(at(rs(dr(xt.wrap("input"))))),pr=class extends hr{get[ee](){return Object.assign(super[ee],{valueCopy:""})}get[ve](){return this.inner}[ke](e){super[ke](e),this[ie]&&this[we].inner.addEventListener("input",(()=>{this[Se]=!0,this.value=this.inner.value,this[Se]=!1}))}get[Ze](){const e=super[Ze];return e.content.append(A`
      <style>
        [part~="input"] {
          font: inherit;
          outline: none;
          text-align: inherit;
        }
      </style>
    `),e}get value(){return super.value}set value(e){const t=String(e);super.value=t,this[Fe]({valueCopy:t})}},mr=function(e){return class extends e{get[ee](){const e=super[ee];return Object.assign(e,{itemRole:e.itemRole||"option",role:e.role||"listbox"})}get itemRole(){return this[Be].itemRole}set itemRole(e){this[Fe]({itemRole:e})}[ke](e){super[ke]&&super[ke](e);const{itemRole:t}=this[Be],s=this[Be].items;if(e.items&&s&&s.forEach((e=>{e.id||(e.id=ks(e))})),(e.items||e.itemRole)&&s&&s.forEach((e=>{t===Ss[e.localName]?e.removeAttribute("role"):e.setAttribute("role",t)})),e.items||e.selectedIndex||e.selectedItemFlags){const{selectedItemFlags:e,selectedIndex:t}=this[Be];s&&s.forEach(((s,n)=>{const i=e?e[n]:n===t;s.setAttribute("aria-selected",i.toString())}))}if(e.items||e.selectedIndex){const{selectedIndex:e}=this[Be],t=e>=0&&s?s[e]:null;t?(t.id||(t.id=ks(t)),this.setAttribute("aria-activedescendant",t.id)):this.removeAttribute("aria-activedescendant")}if(e.selectedItemFlags&&(this[Be].selectedItemFlags?this.setAttribute("aria-multiselectable","true"):this.removeAttribute("aria-multiselectable")),e.orientation){const{orientation:e}=this[Be];this.setAttribute("aria-orientation",e)}if(e.role){const{role:e}=this[Be];this.setAttribute("role",e)}}get role(){return super.role}set role(e){super.role=e,this[De]||this[Fe]({role:e})}}}(tt(Ls(Ms(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{selectedIndex:-1,selectedItem:null})}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};return t.currentIndex?Object.assign(s,{selectedIndex:e.currentIndex}):t.selectedIndex&&Object.assign(s,{currentIndex:e.selectedIndex}),t.currentItem?Object.assign(s,{selectedItem:e.currentItem}):t.selectedItem&&Object.assign(s,{currentItem:e.selectedItem}),s}}}(Fs(at(rs(js(Rs(Ws(os(as(zs($s(ls(Ii(Ei(Pi(Zs(Js(V))))))))))))))))))))),gr=class extends mr{get[ee](){return Object.assign(super[ee],{orientation:"vertical"})}get orientation(){return this[Be].orientation}set orientation(e){this[Fe]({orientation:e})}[ke](e){if(super[ke](e),e.items||e.currentIndex){const{currentIndex:e,items:t}=this[Be];t&&t.forEach(((t,s)=>{t.toggleAttribute("selected",s===e)}))}if(e.orientation){const e="vertical"===this[Be].orientation?{display:"block",flexDirection:"",overflowX:"hidden",overflowY:"auto"}:{display:"flex",flexDirection:"row",overflowX:"auto",overflowY:"hidden"};Object.assign(this[we].container.style,e)}}get[Me](){return this[we].container}get[Ze](){const e=super[Ze];return e.content.append(A`
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
    `),e}},fr=class extends pr{get[ee](){return Object.assign(super[ee],{autoCompleteSelect:!1,opened:!1,originalText:"",textIndex:-1,texts:[]})}[Ee](e,t){if(0===t.length||!e)return null;const s=t.toLowerCase();return e.find((e=>e.toLowerCase().startsWith(s)))||null}get opened(){return this[Be].opened}set opened(e){this[Fe]({opened:e})}[ke](e){if(super[ke](e),this[ie]&&(this[we].inner.addEventListener("input",(()=>{setTimeout((()=>{this[Se]=!0;const e=this.inner,t=this.value.toLowerCase(),s=e.selectionStart===t.length&&e.selectionEnd===t.length,n=this[Be].originalText,i=t.startsWith(n)&&t.length===n.length+1;s&&i&&function(e){const t=e[Ee](e.texts,e.value);t&&e[Fe]({autoCompleteSelect:!0,value:t})}(this),this[Fe]({originalText:t}),this[Se]=!1}))})),K(this[we].accessibleList,gr)),e.opened){const{opened:e}=this[Be];this[we].inner.setAttribute("aria-expanded",e.toString())}if(e.texts){const{texts:e}=this[Be],t=null===e?[]:e.map((e=>{const t=document.createElement("div");return t.textContent=e,t}));L(this[we].accessibleList,t)}if(e.textIndex){const{textIndex:e}=this[Be],t=this[we].accessibleList;"currentIndex"in t&&(t.currentIndex=e);const s=t.currentItem,n=s?s.id:null;n?this[ve].setAttribute("aria-activedescendant",n):this[ve].removeAttribute("aria-activedescendant")}}[Ae](e){super[Ae](e);const{autoCompleteSelect:t,originalText:s}=this[Be];if(e.originalText&&t){this[Fe]({autoCompleteSelect:!1,selectionEnd:this[Be].value.length,selectionStart:s.length});const e=new(window.InputEvent||Event)("input",{detail:{originalText:s}});this.dispatchEvent(e)}}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.valueCopy){const{texts:t,valueCopy:n}=e,i=t.indexOf(n);Object.assign(s,{textIndex:i})}return s}get[Ze](){const e=super[Ze],t=e.content.querySelector('[part~="input"]');return t&&(t.setAttribute("aria-autocomplete","both"),t.setAttribute("aria-controls","accessibleList"),t.setAttribute("role","combobox")),e.content.append(A`
      <style>
        #accessibleList {
          height: 0;
          position: absolute;
          width: 0;
        }
      </style>
      <div id="accessibleList" tabindex="-1"></div>
    `),e}get texts(){return this[Be].texts}set texts(e){this[Fe]({texts:e})}get value(){return super.value}set value(e){super.value=e,this[je]&&!this.inner.matches(":focus")&&this[Fe]({originalText:e})}},br=class extends V{[ke](e){super[ke](e),this[ie]&&this.setAttribute("hidden","")}},yr=ct(st(function(e){return class extends e{select(){this[ve].select()}get selectionEnd(){return this[ve].selectionEnd}set selectionEnd(e){this[ve].selectionEnd=e}get selectionStart(){return this[ve].selectionStart}set selectionStart(e){this[ve].selectionStart=e}setRangeText(...e){this[ve].setRangeText(...e)}setSelectionRange(...e){this[ve].setSelectionRange(...e),document.createElement("input").select}}}(at(rs(as(nn($n(Mn))))))));function wr(e,t,s){if(!s||s.inputPartType){const{inputPartType:s}=t,n=e.getElementById("input");n&&K(n,s)}}function vr(e,t){const s=e[xe];if(!s[t])return!1;const n=s[t]();if(n){const t=s.currentIndex;e[Fe]({currentIndex:t})}return n}const xr=Symbol("itemsChangedListener"),Tr=Symbol("previousItemsDelegate"),Er=Symbol("currentIndexChangedListener"),Pr=Ls(function(e){return class extends e{[ce](){return vr(this,ce)}[ue](){return vr(this,ue)}[he](){return vr(this,he)}[pe](){return vr(this,pe)}}}(function(e){return class extends e{constructor(){super(),this[xr]=e=>{const t=e.target.items;this[Be].items!==t&&this[Fe]({items:t})},this[Er]=e=>{const t=e.detail.currentIndex;this[Be].currentIndex!==t&&this[Fe]({currentIndex:t})}}get[ee](){return Object.assign(super[ee]||{},{items:null})}get items(){return this[Be]?this[Be].items:null}[ke](e){if(super[ke]&&super[ke](e),e.currentIndex){if(void 0===this[xe])throw`To use DelegateItemsMixin, ${this.constructor.name} must define a getter for [itemsDelegate].`;"currentIndex"in this[xe]&&(this[xe].currentIndex=this[Be].currentIndex)}}[Ae](e){super[Ae]&&super[Ae](e);const t=this[Tr];this[xe]!==t&&(t&&(t.removeEventListener(this[xr]),t.removeEventListener(this[Er])),this[xe].addEventListener("itemschange",this[xr]),this[xe].addEventListener("currentindexchange",this[Er]))}}}(Hn(Ii(class extends yr{get[ee](){return Object.assign(super[ee],{ariaHasPopup:null,confirmedValue:"",focused:!1,inputPartType:"input",orientation:"vertical",placeholder:"",selectText:!1,value:""})}get[ve](){return this[we].input}get input(){return this[je]?this[we].input:null}get inputPartType(){return this[Be].inputPartType}set inputPartType(e){this[Fe]({inputPartType:e})}[Te](e){let t;switch(e.key){case"ArrowDown":case"ArrowUp":case"PageDown":case"PageUp":this.closed&&(this.open(),t=!0);break;case"Enter":this.opened||(this.open(),t=!0);break;case"Escape":this.close({canceled:"Escape"}),t=!0;break;case"F4":this.opened?this.close({canceled:"F4"}):this.open(),t=!0}return t||super[Te]&&super[Te](e)}get placeholder(){return this[Be].placeholder}set placeholder(e){this[Fe]({placeholder:String(e)})}[ke](e){if(super[ke](e),wr(this[je],this[Be],e),e.inputPartType&&(this[we].input.addEventListener("blur",(()=>{this[Fe]({focused:!1}),this.opened&&(this[Se]=!0,this.close(),this[Se]=!1)})),this[we].input.addEventListener("focus",(()=>{this[Se]=!0,this[Fe]({focused:!0}),this[Se]=!1})),this[we].input.addEventListener("input",(()=>{this[Se]=!0;const e=this[we].input.value,t={value:e,selectText:!1};this.closed&&e>""&&(t.opened=!0),this[Fe](t),this[Se]=!1})),this[we].input.addEventListener("keydown",(()=>{this[Se]=!0,this[Fe]({selectText:!1}),this[Se]=!1})),this[we].input.addEventListener("mousedown",(e=>{0===e.button&&(this[Se]=!0,this[Fe]({selectText:!1}),this.closed&&!this.disabled&&this.open(),this[Se]=!1)}))),e.opened||e.inputPartType){const e=this[we].input;if("opened"in e){const{opened:t}=this[Be];e.opened=t}}if(e.popupTogglePartType){const e=this[we].popupToggle,t=this[we].input;e.addEventListener("mousedown",(e=>{0===e.button&&(this[Be].disabled?e.preventDefault():(this[Se]=!0,this.toggle(),this[Se]=!1))})),e instanceof HTMLElement&&t instanceof HTMLElement&&E(e,t)}if(e.popupPartType){const e=this[we].popup,t=e;e.removeAttribute("tabindex"),"backdropPartType"in e&&(t.backdropPartType=br),"autoFocus"in e&&(t.autoFocus=!1);const s=t.frame;s&&Object.assign(s.style,{display:"flex",flexDirection:"column"}),"closeOnWindowResize"in e&&(t.closeOnWindowResize=!1)}if(e.disabled){const{disabled:e}=this[Be];this[we].input.disabled=e,this[we].popupToggle.disabled=e}if(e.placeholder){const{placeholder:e}=this[Be];this[we].input.placeholder=e}if(e.popupPosition||e.popupTogglePartType){const{popupPosition:e}=this[Be],t="below"===e?"down":"up",s=this[we].popupToggle;"direction"in s&&(s.direction=t)}if(e.value){const{value:e}=this[Be];this[we].input.value=e}}[Ae](e){super[Ae](e),this[Be].selectText&&setTimeout((()=>{if(this[Be].selectText){const e=this[we].input;e.value>""&&(e.selectionStart=0,e.selectionEnd=e.value.length)}}))}[Ne](e,t){const s=super[Ne](e,t);if(t.opened||t.value){const{closeResult:t,opened:n}=e;n||(t&&t.canceled?Object.assign(s,{value:e.confirmedValue}):Object.assign(s,{confirmedValue:e.value}))}if(t.opened&&!e.opened){const e=!matchMedia("(pointer: coarse)").matches;Object.assign(s,{selectText:e})}return s}get[Ze](){const e=super[Ze],t=e.content.querySelector('slot[name="source"]');return t&&t.replaceWith(A`
        <input id="input" part="input"></input>
      `),wr(e.content,this[Be]),e.content.append(A`
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
      `),e}get value(){return this[Be].value}set value(e){this[Fe]({value:e})}})))));function Ir(e,t,s){if(!s||s.listPartType){const{listPartType:s}=t,n=e.getElementById("list");n&&K(n,s)}}const Cr=Ws(class extends Pr{get[ee](){return Object.assign(super[ee],{currentIndex:-1,horizontalAlign:"stretch",listPartType:gr,selectedIndex:-1,selectedItem:null})}[oe](e){return Ns(e)}[Te](e){let t;const s=this[we].list;switch(e.key){case"ArrowDown":this.opened&&(t=e.altKey?this[ue]():this[he]());break;case"ArrowUp":this.opened&&(t=e.altKey?this[ce]():this[pe]());break;case"PageDown":this.opened&&(t=s.pageDown&&s.pageDown());break;case"PageUp":this.opened&&(t=s.pageUp&&s.pageUp())}if(t){const{selectedIndex:e}=this[Be];e!==s.currentIndex&&this[Fe]({selectedIndex:s.currentIndex})}return t||super[Te]&&super[Te](e)}get listPartType(){return this[Be].listPartType}set listPartType(e){this[Fe]({listPartType:e})}get[xe](){return this[we].list}[ke](e){if(e.listPartType&&this[we].list&&E(this[we].list,null),super[ke](e),Ir(this[je],this[Be],e),e.listPartType){const e=this[we].list;e instanceof HTMLElement&&E(e,this)}}[Ae](e){super[Ae](e),e.listPartType&&this[Fe]({popupList:this[we].list})}get selectedItemValue(){const{items:e,selectedIndex:t}=this[Be],s=e?e[t]:null;return s?s.getAttribute("value"):""}set selectedItemValue(e){const{items:t}=this[Be],s=String(e),n=t.findIndex((e=>e.getAttribute("value")===s));this[Fe]({selectedIndex:n})}[Ne](e,t){const s=super[Ne](e,t);if(t.selectedIndex&&Object.assign(s,{currentIndex:e.selectedIndex}),t.selectedItem&&Object.assign(s,{currentItem:e.selectedItem}),t.items||t.value){const{value:t}=e,n=e.items;if(n&&null!=t){const e=t.toLowerCase(),i=n.findIndex((t=>this[oe](t).toLowerCase()===e));Object.assign(s,{currentIndex:i})}}if(t.selectedIndex){const{items:t,selectedIndex:n,value:i}=e,r=t?t[n]:null,o=r?this[oe](r):"",a=!matchMedia("(pointer: coarse)").matches;i!==o&&Object.assign(s,{selectText:a,value:o})}if(t.opened){const{closeResult:n,currentIndex:i,opened:r}=e,o=t.opened&&!r,a=n&&n.canceled;o&&!a&&i>=0&&Object.assign(s,{selectedIndex:i})}return t.items&&Object.assign(s,{popupMeasured:!1}),s}get[Ze](){const e=super[Ze],t=e.content.querySelector("slot:not([name])");return t&&t.replaceWith(A`
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
    `),Ir(e.content,this[Be]),e}});function Sr(e){return function(e){return e.normalize("NFD").replace(/[\u0300-\u036f]/g,"")}(e).toLowerCase()}const kr=class extends gr{get[ee](){return Object.assign(super[ee],{availableItemFlags:null,filter:null})}get filter(){return this[Be].filter}set filter(e){const t=this[Se];this[Se]=!0,this[Fe]({filter:String(e)}),this[Se]=t}highlightTextInItem(e,t){const s=t.textContent||"",n=e?Sr(s).indexOf(Sr(e)):-1;if(n>=0){const t=n+e.length,i=s.substr(0,n),r=s.substring(n,t),o=s.substr(t),a=document.createDocumentFragment(),l=document.createElement("strong");return l.textContent=r,a.append(new Text(i),l,new Text(o)),a.childNodes}return[new Text(s)]}itemMatchesFilter(e,t){const s=this[oe](e);if(t){if(s){const e=Sr(s),n=Sr(t);return e.includes(n)}return!1}return!0}[ke](e){if(super[ke](e),e.filter||e.items){const{filter:e,availableItemFlags:t,items:s}=this[Be];s&&s.forEach(((s,n)=>{const i=t[n];s.style.display=i?"":"none",i&&L(s,this.highlightTextInItem(e,s))}))}}[Ae](e){super[Ae](e),e.filter&&this.scrollCurrentItemIntoView()}[Ne](e,t){const s=super[Ne]?super[Ne](e,t):{};if(t.filter||t.items){const{filter:t,items:n}=e,i=null===n?null:n.map((e=>this.itemMatchesFilter(e,t)));Object.assign(s,{availableItemFlags:i})}return s}},Lr=Xs(class extends Cr{get[ee](){return Object.assign(super[ee],{inputPartType:fr})}[ke](e){super[ke](e),e.texts&&"texts"in this[we].input&&(this[we].input.texts=this[Be].texts)}}),Or=class extends Lr{get[ee](){return Object.assign(super[ee],{filter:"",listPartType:kr})}[ke](e){if(super[ke](e),e.inputPartType&&this[we].input.addEventListener("input",(e=>{this[Se]=!0;const t=e,s=t.detail?t.detail.originalText:this[Be].value;this[Fe]({filter:s}),this[Se]=!1})),e.filter||e.currentIndex){const{filter:e,currentIndex:t}=this[Be];if(""===e||-1===t){const t=this[we].list;"filter"in t&&(t.filter=e)}}}[Ne](e,t){const s=super[Ne](e,t);return t.opened&&!e.opened&&Object.assign(s,{filter:""}),s}};function Ar(e){return class extends e{get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}}}class Dr extends(Ar(fr)){}const Mr=Dr;class Fr extends(Ar(pr)){}const jr=Fr;class Rr extends(function(e){return class extends e{get[Ze](){const e=super[Ze];return e.content.append(A`
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
        `),e}}}(kr)){}const Hr=Rr;class Br extends(function(e){return class extends e{get[ee](){return Object.assign(super[ee]||{},{inputPartType:jr,popupPartType:Jn,popupTogglePartType:Kn})}[ke](e){if(super[ke]&&super[ke](e),e.inputPartType){const e=this[we].input,t="inner"in e?e.inner:e;Object.assign(t.style,{outline:"none"})}if(e.calculatedPopupPosition){const{calculatedPopupPosition:e,opened:t}=this[Be],s="10px",n="below"===e?`polygon(0px 0px, 100% 0px, 100% -${s}, calc(100% + ${s}) -${s}, calc(100% + ${s}) calc(100% + ${s}), -${s} calc(100% + ${s}), -${s} -${s}, 0px -${s})`:`polygon(-${s} -${s}, calc(100% + ${s}) -${s}, calc(100% + ${s}) calc(100% + ${s}), 100% calc(100% + ${s}), 100% 100%, 0px 100%, 0px calc(100% + ${s}), -${s} calc(100% + ${s}))`;this[we].popup.style.clipPath=t?n:""}}get[Ze](){const e=super[Ze];return e.content.append(A`
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
      `),e}}}(Or)){get[ee](){return Object.assign(super[ee],{inputPartType:Mr,listPartType:Hr})}}const Nr=Br;customElements.define("elix-filter-combo-box",class extends Nr{});const Wr=rs(Xs(dr(xt.wrap("textarea")))),zr=class extends Wr{attributeChangedCallback(e,t,s){"minimum-rows"===e?this.minimumRows=Number(s):super.attributeChangedCallback(e,t,s)}get[ee](){return Object.assign(super[ee],{minimumRows:1,value:null,valueTracksContent:!0})}get minimumRows(){return this[Be].minimumRows}set minimumRows(e){isNaN(e)||this[Fe]({minimumRows:e})}[ke](e){super[ke](e),this[ie]&&this[we].inner.addEventListener("input",(()=>{this[Se]=!0,this[Fe]({valueTracksContent:!1});const e=this[we].inner;this[Fe]({value:e.value}),this[Se]=!1}));const{copyStyle:t,lineHeight:s,minimumRows:n,value:i}=this[Be];if(e.copyStyle&&Object.assign(this[we].copyContainer.style,t),e.lineHeight||e.minimumRows&&null!=s){const e=n*s;this[we].copyContainer.style.minHeight=e+"px"}e.value&&(this[we].inner.value=i,this[we].textCopy.textContent=i)}[Ae](e){if(super[Ae](e),this[ie]){const e=getComputedStyle(this[we].inner),t=this[we].extraSpace.clientHeight;this[Fe]({copyStyle:{"border-bottom-style":e.borderBottomStyle,"border-bottom-width":e.borderBottomWidth,"border-left-style":e.borderLeftStyle,"border-left-width":e.borderLeftWidth,"border-right-style":e.borderRightStyle,"border-right-width":e.borderRightWidth,"border-top-style":e.borderTopStyle,"border-top-width":e.borderTopWidth,"padding-bottom":e.paddingBottom,"padding-left":e.paddingLeft,"padding-right":e.paddingRight,"padding-top":e.paddingTop},lineHeight:t})}if(e.value&&this[Se]){const{value:e}=this[Be],t=new CustomEvent("value-changed",{bubbles:!0,detail:{value:e}});this.dispatchEvent(t);const s=new CustomEvent("input",{bubbles:!0,detail:{value:e}});this.dispatchEvent(s)}}[Ne](e,t){const s=super[Ne](e,t);if((t.content||t.valueTracksContent)&&e.valueTracksContent){const t=function(e){if(null===e)return"";const t=[...e].map((e=>e.textContent));return t.join("").trim().replace(/&amp;/g,"&").replace(/&lt;/g,"<").replace(/&gt;/g,">").replace(/&quot;/g,'"').replace(/&#039;/g,"'")}(e.content);Object.assign(s,{value:t})}return s}get[Ze](){return D.html`
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
    `}get value(){return this[Be].value}set value(e){this[Fe]({value:String(e),valueTracksContent:!1})}},Yr=class extends zr{};customElements.define("elix-auto-size-textarea",class extends Yr{});const Ur=-1!==navigator.vendor.indexOf("Apple")||-1!==navigator.userAgent.indexOf("Edge")?e=>Promise.resolve().then(e):requestAnimationFrame,qr=(e,t,s,n)=>`translate(${e}px, ${t}px) scale(${s}, ${n})`,Vr=(e,t,s=1e-5)=>Math.abs(e-t)<=s,$r=/matrix\((-?\d*\.?\d+),\s*0,\s*0,\s*(-?\d*\.?\d+),\s*0,\s*0\)/,_r="dom-flip-transitioning",Gr=document.createElement("template");Gr.innerHTML="\n    <style>\n        ::slotted(.dom-flip-transitioning) {\n            transition: var(--transition, transform 200ms ease-out, opacity 200ms ease-out);\n        }\n    </style>\n\n    <slot></slot>\n",window.ShadyCSS&&window.ShadyCSS.prepareTemplate(Gr,"dom-flip");class Kr extends HTMLElement{constructor(){super(),this._active=!0,this._attrName="data-flip-id",this._animationEnqueued=!1,this._childData=new Map,this._childObserver=new MutationObserver((()=>this._enqueueAnimateChangedElements())),this._mutationObserverUpdateHandler=()=>this._updateMutationObserver()}static get observedAttributes(){return["active","attr-name"]}get active(){return this._active}set active(e){this._active=e,this._updateListeners()}get attrName(){return this._attrName}set attrName(e){this._attrName=e,this._updateListeners()}connectedCallback(){window.ShadyCSS&&window.ShadyCSS.styleElement(this),this.shadowRoot||this.attachShadow({mode:"open"}),this.shadowRoot.appendChild(Gr.content.cloneNode(!0)),this._slot=this.shadowRoot.querySelector("slot"),this._updateListeners()}attributeChangedCallback(e,t,s){switch(e){case"active":this.active=null!=s;break;case"attr-name":this.attrName=s}}refresh(){Ur((()=>this._childData=this._collectChildData()))}_animateChangedElements(){const e=this._collectChildData();for(const[t,[s,n]]of e.entries()){const e=this._childData.get(t);if(!e)continue;const[i,r]=e,o=r.top-n.top,a=r.left-n.left;Vr(o,0)&&Vr(a,0)&&Vr(r.scaleX/n.scaleX,1)&&Vr(r.scaleY/n.scaleY,1)||(s.classList.remove(_r),s.style.opacity=String(r.opacity),s.style.transform=qr(a,o,r.scaleX,r.scaleY),requestAnimationFrame((()=>{s.style.opacity=String(n.opacity),s.style.transform=qr(0,0,n.scaleX,n.scaleY),s.classList.add(_r)})))}this._childData=e}_collectChildData(){const e=this.getBoundingClientRect(),t=new Map;for(const s of this._slot.assignedNodes()){if(!(s instanceof HTMLElement))continue;const n=s.getAttribute(this.attrName);if(!n)continue;const i=s.getBoundingClientRect(),r=window.getComputedStyle(s);let o="1.0",a="1.0";const l=r.transform.match($r);l&&([,o,a]=l);const c={top:i.top-e.top,left:i.left-e.left,opacity:Number.parseFloat(r.opacity||"1"),scaleX:Number.parseFloat(o),scaleY:Number.parseFloat(a)};t.set(n,[s,c])}return t}_enqueueAnimateChangedElements(){this._animationEnqueued||(this._animationEnqueued=!0,Ur((()=>{this._animationEnqueued=!1,this._animateChangedElements()})))}_updateListeners(){this.removeEventListener("dom-change",this._mutationObserverUpdateHandler),this._slot.removeEventListener("slotchange",this._mutationObserverUpdateHandler),this.active&&(this.addEventListener("dom-change",this._mutationObserverUpdateHandler),this._slot.addEventListener("slotchange",this._mutationObserverUpdateHandler)),this._updateMutationObserver()}_updateMutationObserver(){this._childObserver.disconnect(),this.active&&(this._slot.assignedNodes().filter((e=>e instanceof HTMLElement)).forEach((e=>this._childObserver.observe(e,{attributes:!0,attributeFilter:[this.attrName]}))),this._enqueueAnimateChangedElements())}}customElements.define("dom-flip",Kr)})();